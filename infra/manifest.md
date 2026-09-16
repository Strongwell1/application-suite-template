# Application infrastructure manifest

This manifest is the per-instantiation source of truth for the values an agent
needs to configure the OpenTofu infrastructure skeleton. It also summarizes
the template's naming conventions and minimum floors. The OpenTofu module is
the executable authority for derived names and validation constraints.

Every unresolved value uses a `REPLACE_WITH_*` token. This is the only
placeholder format; searching for `REPLACE_WITH_` finds every value that still
requires substitution.

## Application

| Value | Assignment | Notes |
| --- | --- | --- |
| Application name | `REPLACE_WITH_APPLICATION_NAME` | Lowercase kebab-case; used in conventional Azure resource names. |
| SQL database name | `REPLACE_WITH_DATABASE_NAME` | Logical application database name, conventionally PascalCase. |
| Blob container name | `REPLACE_WITH_BLOB_CONTAINER_NAME` | Lowercase DNS-style name shared across environments. |
| Organization name | `Strongwell` | Used in resource tags. |

## Environments

Environment names are always lowercase: `dev`, `uat`, and `prod`.

| Environment | Tenant ID | Subscription ID | Location | App Service plan SKU | SQL SKU |
| --- | --- | --- | --- | --- | --- |
| `dev` | `e783372a-ac75-4ad0-a340-9202cd1be965` | `230bf07e-199a-4e7c-875f-b994a5794bfa` | `Central US` | `B1` | `GP_S_Gen5_1` |
| `uat` | `06aa107f-ff7e-449e-a4d1-11e8b3585c11` | `7f979b1e-fe00-41ab-9777-31b75c24828f` | `East US 2` | `B1` | `GP_S_Gen5_1` |
| `prod` | `06aa107f-ff7e-449e-a4d1-11e8b3585c11` | `7f979b1e-fe00-41ab-9777-31b75c24828f` | `East US 2` | `P2v3` | `GP_S_Gen5_2` |

## Storage accounts

Storage account names must be globally unique and contain 3-24 lowercase
alphanumeric characters. Record the confirmed name for each environment here.

| Environment | Storage account name |
| --- | --- |
| `dev` | `REPLACE_WITH_DEV_STORAGE_ACCOUNT_NAME` |
| `uat` | `REPLACE_WITH_UAT_STORAGE_ACCOUNT_NAME` |
| `prod` | `REPLACE_WITH_PROD_STORAGE_ACCOUNT_NAME` |

## SQL behavior

| Environment | Maximum size | Auto-pause | PITR retention | Long-term retention | Backup redundancy |
| --- | --- | --- | --- | --- | --- |
| `dev` | 2 GB | 60 minutes | 7 days | disabled | `REPLACE_WITH_DEV_SQL_BACKUP_REDUNDANCY` |
| `uat` | 2 GB | 60 minutes | 7 days | disabled | `REPLACE_WITH_UAT_SQL_BACKUP_REDUNDANCY` |
| `prod` | 64 GB | disabled | 31 days | 13 weekly, 6 monthly, 2 yearly (week 1) | `GeoZone` |

## Application Resource Inventory

This inventory defines the minimum resources provisioned by the skeleton.
Every listed resource is required.

| Resource | Why |
| --- | --- |
| Resource Group | Container for every other resource |
| App Service Plan | Hosts the API |
| Web App (API) | REST API backend |
| Static Web App (SPA) | Frontend |
| SQL Database | Relational data |
| Blob Storage | Application file and object storage |

## Naming

The module derives resource names from the application, environment, and an
eight-character lowercase hexadecimal hash:

`hash = substr(sha256("<subscription-id>-<environment>"), 0, 8)`

| Resource | Pattern |
| --- | --- |
| Resource Group | `rg-app-<application-name>-<environment>` |
| App Service Plan | `asp-<application-name>-<environment>` |
| Web App (API) | `wa-<application-name>-api-<environment>-<hash>` |
| Static Web App (SPA) | `swa-<application-name>-spa-<environment>-<hash>` |
| SQL Server | `sql-<application-name>-<environment>-<hash>` |
| SQL Database | Confirmed application value from this manifest |
| Storage Account | Confirmed environment value from this manifest |
| Blob Container | Confirmed application value from this manifest |

The hash formula is part of resource identity: changing it can cause OpenTofu
to propose replacement resources. Storage account names remain explicit
because Azure requires global uniqueness and limits them to 3-24 lowercase
alphanumeric characters. If this summary and `infra/modules/main.tf` differ,
the module describes current behavior and this manifest must be corrected.

### Exceptions

Record only ratified deviations after Azure rejects a convention-derived name.
Each entry must identify the resource, environment, literal replacement name,
and reason. The agent then updates the relevant `.tf` assignment to match.

None recorded yet.

## Values requiring confirmation

| Environment | API Entra audience |
| --- | --- |
| `dev` | `REPLACE_WITH_DEV_API_AUDIENCE` |
| `uat` | `REPLACE_WITH_UAT_API_AUDIENCE` |
| `prod` | `REPLACE_WITH_PROD_API_AUDIENCE` |

## SQL firewall rules

| Name | Start IP address | End IP address |
| --- | --- | --- |
| Allow Azure services | `0.0.0.0` | `0.0.0.0` |
| Allow BRI Lumen | `4.4.234.130` | `4.4.234.130` |
| Allow BRI Spectrum | `35.130.100.137` | `35.130.100.142` |
