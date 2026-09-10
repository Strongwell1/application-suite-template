# Application infrastructure manifest

This manifest is the per-instantiation source of truth for the values an agent
needs to configure the OpenTofu infrastructure skeleton. Durable policy,
naming formulas, validation constraints, and minimum floors belong in
`INFRA.md`; confirmed choices and exceptions belong here.

## Application

| Value | Assignment | Notes |
| --- | --- | --- |
| Application name | `[application-name]` | Lowercase kebab-case; used in conventional Azure resource names. |
| SQL database name | `[database-name]` | Logical application database name, conventionally PascalCase. |
| Organization name | `Strongwell` | Used in resource tags. |

## Environments

Environment names are always lowercase: `dev`, `uat`, and `prod`.

| Environment | Tenant ID | Subscription ID | Location | App Service plan SKU | SQL SKU |
| --- | --- | --- | --- | --- | --- |
| `dev` | `e783372a-ac75-4ad0-a340-9202cd1be965` | `230bf07e-199a-4e7c-875f-b994a5794bfa` | `Central US` | `B1` | `GP_S_Gen5_1` |
| `uat` | `06aa107f-ff7e-449e-a4d1-11e8b3585c11` | `7f979b1e-fe00-41ab-9777-31b75c24828f` | `East US 2` | `B1` | `GP_S_Gen5_1` |
| `prod` | `06aa107f-ff7e-449e-a4d1-11e8b3585c11` | `7f979b1e-fe00-41ab-9777-31b75c24828f` | `East US 2` | `P2v3` | `GP_S_Gen5_2` |

## SQL behavior

| Environment | Maximum size | Auto-pause | PITR retention | Long-term retention | Backup redundancy |
| --- | --- | --- | --- | --- | --- |
| `dev` | 2 GB | 60 minutes | 7 days | disabled | `[confirm]` |
| `uat` | 2 GB | 60 minutes | 7 days | disabled | `[confirm]` |
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

Derive resource names from the authoritative conventions in `/rules/tech/INFRA.md`.
Do not duplicate those formulas here.

### Exceptions

Record only ratified deviations after Azure rejects a convention-derived name.
Each entry must identify the resource, environment, literal replacement name,
and reason. The agent then updates the relevant `.tf` assignment to match.

None recorded yet.

## Values requiring confirmation

- API Entra audience: `REPLACE_WITH_API_AUDIENCE`

## SQL firewall rules

| Name | Start IP address | End IP address |
| --- | --- | --- |
| Allow Azure services | `0.0.0.0` | `0.0.0.0` |
| Allow BRI Lumen | `4.4.234.130` | `4.4.234.130` |
| Allow BRI Spectrum | `35.130.100.137` | `35.130.100.142` |
