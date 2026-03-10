# Azure Terraform Configuration for SanlamBank

This Terraform project creates:

- Azure Resource Group
- Azure Key Vault
- Key Vault secrets for configuration values

## Usage

### 1. Login to Azure

az login

### 2. Initialize Terraform

terraform init

### 3. Copy variables file

cp terraform.tfvars

Update the tenant_id if required.

### 4. Deploy infrastructure

terraform apply

Terraform will create:
- Resource Group
- Key Vault
- Secrets storing SanlamBank configuration values