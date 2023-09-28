variable "resource_group_name" {
  type        = string
  default     = ""
  description = "The resource group name to override default naming convention"
}

variable "environment" {
  type        = string
  default     = "dev"
  description = "The environment name"
  validation {
    condition     = can(regex("dev|stag|prod", var.environment))
    error_message = "The environment name value is not valid."
  }
}

variable "domain" {
  type        = string
  default     = "rds"
  description = "The domain name"
}

variable "location" {
  type        = string
  default     = "westeurope"
  description = "The Azure region where the resources should be created"
}

variable "tags" {
  type        = map(any)
  description = "The custom tags for all resources"
  default     = {}
}

variable "app_service_plan_sku" {
  type        = string
  default     = "S3"
  description = "The app service plan SKU"
}

variable "apim_sku" {
  type        = string
  default     = "Consumption_0"
  description = "The API Management SKU"
}


