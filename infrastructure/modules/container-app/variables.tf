variable "project_name" {
  description = "Name of the project"
  type        = string
}

variable "environment" {
  description = "Environment name (dev, prod)"
  type        = string
}

variable "location" {
  description = "Azure region"
  type        = string
  default     = "East US"
}

variable "container_app_name" {
  description = "Name of the container app"
  type        = string
}

variable "container_image" {
  description = "Container image to deploy"
  type        = string
}

variable "cpu_requests" {
  description = "CPU requests for the container"
  type        = string
  default     = "0.25"
}

variable "memory_requests" {
  description = "Memory requests for the container"
  type        = string
  default     = "0.5Gi"
}

variable "min_replicas" {
  description = "Minimum number of replicas"
  type        = number
  default     = 1
}

variable "max_replicas" {
  description = "Maximum number of replicas"
  type        = number
  default     = 5
}

variable "environment_variables" {
  description = "Environment variables for the container"
  type        = map(string)
  default     = {}
}

variable "ingress_enabled" {
  description = "Enable ingress for the container app"
  type        = bool
  default     = true
}

variable "target_port" {
  description = "Target port for the ingress"
  type        = number
  default     = 80
}

variable "external_ingress" {
  description = "Enable external ingress"
  type        = bool
  default     = true
}

variable "tags" {
  description = "Tags to apply to resources"
  type        = map(string)
  default     = {}
}
