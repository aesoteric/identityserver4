# Resource group
resource "azurerm_resource_group" "main" {
  name     = "rg-${var.project_name}-${var.environment}"
  location = var.location
  tags     = var.tags
}

# Container registry
resource "azurerm_container_registry" "main" {
  name                = "acr${var.project_name}${var.environment}"
  resource_group_name = azurerm_resource_group.main.name
  location            = azurerm_resource_group.main.location
  sku                 = "Basic"
  admin_enabled       = true
  tags                = var.tags
}

# Log Analytics workspace
resource "azurerm_log_analytics_workspace" "main" {
  name                = "law-${var.project_name}-${var.environment}"
  location            = azurerm_resource_group.main.location
  resource_group_name = azurerm_resource_group.main.name
  sku                 = "PerGB2018"
  retention_in_days   = 30
  tags                = var.tags
}

# Container Apps environment
resource "azurerm_container_app_environment" "main" {
  name                       = "cae-${var.project_name}-${var.environment}"
  location                   = azurerm_resource_group.main.location
  resource_group_name        = azurerm_resource_group.main.name
  log_analytics_workspace_id = azurerm_log_analytics_workspace.main.id
  tags                       = var.tags
}

# Import a placeholder image after ACR creation
resource "null_resource" "import_placeholder_image" {
  depends_on = [azurerm_container_registry.main]

  provisioner "local-exec" {
    command = <<-EOT
      az acr import \
        --name ${azurerm_container_registry.main.name} \
        --source mcr.microsoft.com/hello-world:latest \
        --image hello-world:latest \
        --resource-group ${azurerm_resource_group.main.name}
    EOT
  }

  triggers = {
    acr_id = azurerm_container_registry.main.id
  }
}

# Container App
resource "azurerm_container_app" "main" {
  depends_on = [null_resource.import_placeholder_image]

  name                         = var.container_app_name
  container_app_environment_id = azurerm_container_app_environment.main.id
  resource_group_name          = azurerm_resource_group.main.name
  revision_mode                = "Single"
  tags                         = var.tags

  template {
    min_replicas = var.min_replicas
    max_replicas = var.max_replicas

    container {
      name   = "main"
      image  = "${azurerm_container_registry.main.login_server}/hello-world:latest"
      cpu    = var.cpu_requests
      memory = var.memory_requests

      dynamic "env" {
        for_each = var.environment_variables
        content {
          name  = env.key
          value = env.value
        }
      }
    }
  }

  dynamic "ingress" {
    for_each = var.ingress_enabled ? [1] : []
    content {
      external_enabled = var.external_ingress
      target_port      = var.target_port
      traffic_weight {
        percentage      = 100
        latest_revision = true
      }
    }
  }

  registry {
    server               = azurerm_container_registry.main.login_server
    username             = azurerm_container_registry.main.admin_username
    password_secret_name = "registry-password"
  }

  secret {
    name  = "registry-password"
    value = azurerm_container_registry.main.admin_password
  }

  lifecycle {
    ignore_changes = [template[0].container[0].image]
  }
}
