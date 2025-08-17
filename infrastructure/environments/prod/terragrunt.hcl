# Include the root configuration
include "root" {
  path = find_in_parent_folders("root.hcl")
}

# Include the module configuration
terraform {
  source = "../../modules/container-app"
}

# Environment-specific inputs
inputs = {
  environment = "prod"

  # Container App settings
  container_app_name = "ca-identityserver4-prod"
  container_image    = "identityserver4-prod:latest"
  cpu_requests       = "0.5"
  memory_requests    = "1Gi"
  min_replicas       = 2
  max_replicas       = 10

  # Environment variables for the container
  environment_variables = {
    ENVIRONMENT = "production"
    LOG_LEVEL   = "info"
  }

  # Ingress settings
  ingress_enabled  = true
  target_port      = 3000
  external_ingress = true

  tags = {
    Environment = "prod"
    Project     = "identityserver4"
    ManagedBy   = "terragrunt"
  }
}
