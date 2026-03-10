terraform {
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

resource "azurerm_resource_group" "rg" {
  name     = var.resource_group_name
  location = var.location
}

resource "azurerm_key_vault" "kv" {
  name                        = var.key_vault_name
  location                    = azurerm_resource_group.rg.location
  resource_group_name         = azurerm_resource_group.rg.name
  tenant_id                   = var.tenant_id
  sku_name                    = "standard"
  purge_protection_enabled    = false
  soft_delete_retention_days  = 7
}

locals {
  secrets = {
    "SanlamBank--Redis--ConnectionString" = "127.0.0.1:6379"

    "SanlamBank--Sql--BankConnection"  = "ms-sql-1.connection.sanlam"
    "SanlamBank--Sql--OtherConnection" = "ms-sql-2.connection.sanlam"

    "SanlamBank--NoSql--BankConnection"  = "no-sql-1.connection.sanlam"
    "SanlamBank--NoSql--OtherConnection" = "no-sql-2.connection.sanlam"

    "SanlamBank--Kafka--Password" = "sanlam"

    "SanlamBank--RabbitMQ--Password" = "sanlam"
  }
}

resource "azurerm_key_vault_secret" "config" {
  for_each = local.secrets

  name         = each.key
  value        = each.value
  key_vault_id = azurerm_key_vault.kv.id
}