locals {
  # Most-likely-to-change values first. These assignments are populated from
  # infra/manifest.md for each application-suite instantiation.
  env                  = "dev"
  tenant_id            = "e783372a-ac75-4ad0-a340-9202cd1be965"
  subscription_id      = "230bf07e-199a-4e7c-875f-b994a5794bfa"
  app_name             = "REPLACE_WITH_APPLICATION_NAME"
  audience             = "REPLACE_WITH_DEV_API_AUDIENCE"
  storage_account_name = "REPLACE_WITH_DEV_STORAGE_ACCOUNT_NAME"

  location     = "Central US"
  sql_db_name  = "REPLACE_WITH_DATABASE_NAME"
  asp_sku_name = "B1"
}

module "app" {
  source = "../modules"

  env                  = local.env
  tenant_id            = local.tenant_id
  subscription_id      = local.subscription_id
  app_name             = local.app_name
  audience             = local.audience
  storage_account_name = local.storage_account_name

  location     = local.location
  sql_db_name  = local.sql_db_name
  asp_sku_name = local.asp_sku_name

  tags = {
    app = local.app_name
    env = local.env
    org = "strongwell"
  }
}
