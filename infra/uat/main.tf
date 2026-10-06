locals {
  # Most-likely-to-change values first. These assignments are populated from
  # infra/manifest.md for each application-suite instantiation.
  env                  = "uat"
  tenant_id            = "06aa107f-ff7e-449e-a4d1-11e8b3585c11"
  subscription_id      = "7f979b1e-fe00-41ab-9777-31b75c24828f"
  app_name             = "REPLACE_WITH_APPLICATION_NAME"
  audience             = "REPLACE_WITH_UAT_API_AUDIENCE"
  storage_account_name = "REPLACE_WITH_UAT_STORAGE_ACCOUNT_NAME"
  blob_container_name  = "REPLACE_WITH_BLOB_CONTAINER_NAME"

  location     = "East US 2"
  sql_db_name  = "REPLACE_WITH_DATABASE_NAME"
  asp_sku_name = "B1"

  sql_storage_account_type = "REPLACE_WITH_UAT_SQL_BACKUP_REDUNDANCY"
}

module "app" {
  source = "../modules"

  env                  = local.env
  tenant_id            = local.tenant_id
  subscription_id      = local.subscription_id
  app_name             = local.app_name
  audience             = local.audience
  storage_account_name = local.storage_account_name
  blob_container_name  = local.blob_container_name

  location     = local.location
  sql_db_name  = local.sql_db_name
  asp_sku_name = local.asp_sku_name

  sql_storage_account_type = local.sql_storage_account_type

  tags = {
    app = local.app_name
    env = local.env
    org = "strongwell"
  }
}
