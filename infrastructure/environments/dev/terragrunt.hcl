# Include the root terragrunt.hcl configuration
include "root" {
  path = find_in_parent_folders()
}

# Include the module configuration
terraform {
  source = "../../modules/container-app"
}

# Environment-specific inputs
inputs = {
  environment = "dev"

  # Container App settings
  container_app_name = "ca-myapp-dev"
  container_image    = "myapp:dev"
  cpu_requests       = "0.25"
  memory_requests    = "0.5Gi"
  min_replicas       = 1
  max_replicas       = 3

  # Environment variables for the container
  environment_variables = {
    ENVIRONMENT = "development"
    LOG_LEVEL   = "debug"
  }

  # Ingress settings
  ingress_enabled  = true
  target_port      = 3000
  external_ingress = true

  tags = {
    Environment = "dev"
    Project     = "myapp"
    ManagedBy   = "terragrunt"
  }
}