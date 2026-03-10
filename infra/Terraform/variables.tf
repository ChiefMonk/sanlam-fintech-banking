variable "resource_group_name" {
  description = "Azure Resource Group name"
  type        = string
}

variable "location" {
  description = "Azure region"
  type        = string
  default     = "southafricanorth"
}

variable "key_vault_name" {
  description = "Azure Key Vault name"
  type        = string
}

variable "tenant_id" {
  description = "Azure Tenant ID"
  type        = string
}