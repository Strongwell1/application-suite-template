terraform {
  required_providers {
    azurerm = {
      source = "hashicorp/azurerm"
    }
    azuread = {
      source = "hashicorp/azuread"
    }
  }
}

provider "azurerm" {
  tenant_id                       = var.tenant_id
  subscription_id                 = var.subscription_id
  resource_provider_registrations = "none"
  features {}
}

provider "azuread" {
  tenant_id = var.tenant_id
}

locals {
  hash            = substr(sha256("${var.subscription_id}-${var.env}"), 0, 8)
  rg_name         = "rg-app-${var.app_name}-${var.env}"
  asp_name        = "asp-${var.app_name}-${var.env}"
  sql_server_name = "sql-${var.app_name}-${var.env}-${local.hash}"
  sql_db_name     = var.sql_db_name
  wa_name         = "wa-${var.app_name}-api-${var.env}-${local.hash}"
  swa_name        = "swa-${var.app_name}-spa-${var.env}-${local.hash}"
  container_name  = "attachments"
  app_environment_name = lookup({
    dev  = "dev"
    uat  = "uat"
    prod = "prod"
  }, var.env, "development")
  sql_connection_string = "Server=tcp:${azurerm_mssql_server.sql.fully_qualified_domain_name},1433;Initial Catalog=${local.sql_db_name};Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Authentication=\"Active Directory Managed Identity\";"
}

resource "azurerm_resource_group" "rg" {
  name     = local.rg_name
  location = var.location
  tags     = var.tags
}

resource "azurerm_service_plan" "app" {
  name                = local.asp_name
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name

  os_type  = "Linux"
  sku_name = var.asp_sku_name

  tags = var.tags
}

data "azuread_client_config" "aad" {}

data "azuread_user" "me" {
  object_id = data.azuread_client_config.aad.object_id
}

resource "azurerm_mssql_server" "sql" {
  name                          = local.sql_server_name
  resource_group_name           = azurerm_resource_group.rg.name
  location                      = azurerm_resource_group.rg.location
  version                       = "12.0"
  minimum_tls_version           = "1.2"
  public_network_access_enabled = true
  tags                          = var.tags

  azuread_administrator {
    login_username              = data.azuread_user.me.user_principal_name
    object_id                   = data.azuread_client_config.aad.object_id
    tenant_id                   = var.tenant_id
    azuread_authentication_only = true
  }
}

resource "azurerm_mssql_firewall_rule" "allow_azure_services" {
  name             = "Allow Azure services"
  server_id        = azurerm_mssql_server.sql.id
  start_ip_address = "0.0.0.0"
  end_ip_address   = "0.0.0.0"
}

resource "azurerm_mssql_firewall_rule" "allow_bri_lumen" {
  name             = "Allow BRI Lumen"
  server_id        = azurerm_mssql_server.sql.id
  start_ip_address = var.bri_lumen_ip
  end_ip_address   = var.bri_lumen_ip
}

resource "azurerm_mssql_firewall_rule" "allow_bri_spectrum" {
  name             = "Allow BRI Spectrum"
  server_id        = azurerm_mssql_server.sql.id
  start_ip_address = var.bri_spectrum_start_ip
  end_ip_address   = var.bri_spectrum_end_ip
}

resource "azurerm_mssql_database" "db" {
  name                        = local.sql_db_name
  server_id                   = azurerm_mssql_server.sql.id
  sku_name                    = var.sql_sku_name
  min_capacity                = var.sql_min_capacity
  max_size_gb                 = var.env == "prod" ? var.sql_prod_max_size_gb : 2
  auto_pause_delay_in_minutes = var.sql_auto_pause_delay
  storage_account_type        = var.sql_storage_account_type
  tags                        = var.tags

  short_term_retention_policy {
    retention_days = var.sql_pitr_days
  }

  long_term_retention_policy {
    weekly_retention  = var.enable_sql_ltr ? "P${var.sql_ltr_weekly_retention}W" : "P0W"
    monthly_retention = var.enable_sql_ltr ? "P${var.sql_ltr_monthly_retention}M" : "P0M"
    yearly_retention  = var.enable_sql_ltr ? "P${var.sql_ltr_yearly_retention}Y" : "P0Y"
    week_of_year      = 1
  }
}

resource "azurerm_storage_account" "storage" {
  name                     = var.storage_account_name
  resource_group_name      = azurerm_resource_group.rg.name
  location                 = azurerm_resource_group.rg.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
  tags                     = var.tags
}

resource "azurerm_storage_container" "attachments" {
  name                  = local.container_name
  storage_account_id    = azurerm_storage_account.storage.id
  container_access_type = "private"
}

resource "azurerm_linux_web_app" "api" {
  name                = local.wa_name
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location
  service_plan_id     = azurerm_service_plan.app.id
  https_only          = true

  # Disable basic-auth publishing; deployments authenticate via Entra OAuth token to OneDeploy.
  ftp_publish_basic_authentication_enabled       = false
  webdeploy_publish_basic_authentication_enabled = false

  client_certificate_enabled = false
  client_certificate_mode    = "Optional"

  tags = var.tags

  identity {
    type = "SystemAssigned"
  }

  site_config {
    application_stack {
      dotnet_version = "10.0"
    }
  }

  app_settings = {
    "ASPNETCORE_ENVIRONMENT"       = local.app_environment_name
    "ASPNETCORE_HTTPS_PORT"        = "443"
    "Authentication__Audience"     = var.audience
    "Authentication__Authority"    = "https://login.microsoftonline.com/${var.tenant_id}/v2.0"
    "Cors__AllowedOrigins__SpaApp" = "https://${azurerm_static_web_app.swa.default_host_name}"
    "App__SpaBaseUrl"              = "https://${azurerm_static_web_app.swa.default_host_name}"
    "AzureStorage__AccountUrl"     = azurerm_storage_account.storage.primary_blob_endpoint
    "AzureStorage__ContainerName"  = azurerm_storage_container.attachments.name
    "AzureStorage__TenantId"       = var.tenant_id
  }

  connection_string {
    name  = var.sql_connection_string_name
    type  = "SQLAzure"
    value = local.sql_connection_string
  }
}

resource "azurerm_role_assignment" "blob_contributor" {
  scope                = "${azurerm_storage_account.storage.id}/blobServices/default/containers/${azurerm_storage_container.attachments.name}"
  role_definition_name = "Storage Blob Data Contributor"
  principal_id         = azurerm_linux_web_app.api.identity[0].principal_id
}

resource "azurerm_static_web_app" "swa" {
  name                = local.swa_name
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location

  sku_tier = "Free"
  sku_size = "Free"

  tags = var.tags
}

output "AzureStorage__AccountUrl" {
  value = azurerm_storage_account.storage.primary_blob_endpoint
}

output "AzureStorage__ContainerName" {
  value = azurerm_storage_container.attachments.name
}
