# Most-likely-to-change values first, static ones below.

variable "env" {
  type = string
}

variable "tenant_id" {
  type = string
}

variable "subscription_id" {
  type = string
}

variable "app_name" {
  type = string
}

variable "audience" {
  description = "Authentication__Audience app setting: the API app registration's Identifier URI."
  type        = string
}

variable "storage_account_name" {
  type = string

  validation {
    condition     = length(var.storage_account_name) >= 3 && length(var.storage_account_name) <= 24 && can(regex("^[a-z0-9]+$", var.storage_account_name))
    error_message = "Storage account name must be 3-24 lowercase alphanumeric characters with no dashes or special characters."
  }
}

variable "location" {
  type = string
}

variable "sql_db_name" {
  type = string
}

variable "asp_sku_name" {
  description = "App Service Plan tier (sku_name)."
  type        = string
}

variable "bri_lumen_ip" {
  description = "SQL firewall allow-list IP for BRI Lumen (SSIS access)."
  type        = string
  default     = "4.4.234.130"
}

variable "bri_spectrum_start_ip" {
  description = "SQL firewall allow-list range start for BRI Spectrum (SSIS access)."
  type        = string
  default     = "35.130.100.137"
}

variable "bri_spectrum_end_ip" {
  description = "SQL firewall allow-list range end for BRI Spectrum (SSIS access)."
  type        = string
  default     = "35.130.100.142"
}

variable "sql_sku_name" {
  type    = string
  default = "GP_S_Gen5_1"
}

variable "sql_min_capacity" {
  type    = number
  default = 0.5
}

variable "sql_auto_pause_delay" {
  type    = number
  default = 60
}

variable "sql_storage_account_type" {
  type    = string
  default = "Local"
}

variable "sql_pitr_days" {
  type    = number
  default = 7
}

variable "enable_sql_ltr" {
  type    = bool
  default = false
}

variable "sql_ltr_weekly_retention" {
  type    = number
  default = 0
}

variable "sql_ltr_monthly_retention" {
  type    = number
  default = 0
}

variable "sql_ltr_yearly_retention" {
  type    = number
  default = 0
}

variable "sql_prod_max_capacity" {
  description = "Prod-only max vCores for SQL DB."
  type        = number
  default     = null
}

variable "sql_prod_max_size_gb" {
  description = "Prod-only max data size in GB for SQL DB."
  type        = number
  default     = 64
}

variable "sql_connection_string_name" {
  type    = string
  default = "DefaultConnection"
}

variable "tags" {
  type    = map(string)
  default = {}
}
