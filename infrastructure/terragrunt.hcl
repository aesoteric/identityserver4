# Configure Terragrunt to automatically retry on errors
retryable_errors = [
  "(?s).*Error installing provider.*tcp.*connection reset by peer.*",
  "(?s).*ssh_exchange_identification.*Connection closed by remote host.*"
]

# Configure remote state
remote_state {
  backend = "azurerm"
  generate = {
    path      = "backend.tf"
    if_exists = "overwrite_terragrunt"
  }
  config = {
    resource_group_name  = "rg-terraform-state"
    storage_account_name = "sttfstate${get_env("TF_VAR_project_name", "myapp")}"
    container_name       = "tfstate"
    key                  = "${path_relative_to_include()}/terraform.tfstate"
  }
}

# Generate provider configuration
generate "provider" {
  path      = "provider.tf"
  if_exists = "overwrite_terragrunt"
  contents  = <<EOF
terraform {
  required_version = ">= 1.0"
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 3.0"
    }
  }
}

provider "azurerm" {
  features {}
}
EOF
}

# Set common inputs for all environments
inputs = {
  project_name = get_env("TF_VAR_project_name", "myapp")
  location     = get_env("TF_VAR_location", "East US")
}