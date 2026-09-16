locals {
  # Most-likely-to-change values first. These assignments are populated from
  # infra/manifest.md for each application-suite instantiation.
  env                  = "prod"
  tenant_id            = "06aa107f-ff7e-449e-a4d1-11e8b3585c11"
  subscription_id      = "7f979b1e-fe00-41ab-9777-31b75c24828f"
  app_name             = "REPLACE_WITH_APPLICATION_NAME"
  audience             = "REPLACE_WITH_PROD_API_AUDIENCE"
  storage_account_name = "REPLACE_WITH_PROD_STORAGE_ACCOUNT_NAME"

  location     = "East US 2"
  sql_db_name  = "REPLACE_WITH_DATABASE_NAME"
  asp_sku_name = "P2v3"

  # Production SQL Database minimum floor.
  sql_sku_name              = "GP_S_Gen5_2"
  sql_auto_pause_delay      = -1
  sql_pitr_days             = 31
  enable_sql_ltr            = true
  sql_ltr_weekly_retention  = 13
  sql_ltr_monthly_retention = 6
  sql_ltr_yearly_retention  = 2
  sql_storage_account_type  = "GeoZone"
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

  sql_sku_name              = local.sql_sku_name
  sql_auto_pause_delay      = local.sql_auto_pause_delay
  sql_pitr_days             = local.sql_pitr_days
  enable_sql_ltr            = local.enable_sql_ltr
  sql_ltr_weekly_retention  = local.sql_ltr_weekly_retention
  sql_ltr_monthly_retention = local.sql_ltr_monthly_retention
  sql_ltr_yearly_retention  = local.sql_ltr_yearly_retention
  sql_storage_account_type  = local.sql_storage_account_type

  tags = {
    app = local.app_name
    env = local.env
    org = "strongwell"
  }
}
