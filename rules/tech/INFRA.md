# Infra Tech Rules

Conventions specific to `infra/`, plus environment topology and cloud-provider specifics — see `manifest.md` for per-instantiation values.

## Cloud provider

- Azure — uat and prod tenants (Production tenant): all agent operations, including reads, remain fully and unconditionally prohibited (see the repository agent instructions under Hard Prohibitions). This is a hard prohibition regardless of `engage`.

- Azure — Dev tenant only: read-only agent access is permitted, via a pre-configured Reader-role service principal invoked through the Azure CLI (`az`). Scope is validating OpenTofu-provisioned resources and Entra ID app registrations/enterprise applications — not a general-purpose Azure read capability.

- Permitted resource types (list/show-only, enforced by the agent harness allowlist): Resource Groups, App Service Plans, Web Apps, Static Web Apps, SQL Server/DB, Storage Accounts/Containers/Blobs, generic `az resource` queries, Entra app registrations (`az ad app`), and enterprise applications/service principals (`az ad sp`). This list is expected to grow as new resource types are provisioned via OpenTofu — additions are ordinary agent-harness configuration changes under the same harness-change discipline, not a fresh Hard Prohibition carve-out each time.

- Explicitly excluded regardless of the above, via the agent harness denylist: anything that surfaces secrets, credentials, connection strings, or keys (app settings, connection strings, credential lists, storage account keys, SQL connection strings). Also excluded: directory-wide Entra reads — users, groups, and group membership are outside the agent's purpose here.

- The agent never runs `az login` or any credential-provisioning step itself — the service principal's authenticated session is established outside anything the agent executes.

## IaC tool

- OpenTofu (`tofu` CLI) — a compatible, open-source fork of Terraform. The agent must never run `tofu init`, `plan`, `apply`, `destroy`, `import`, `state`, or any subcommand (see the repository agent instructions under Hard Prohibitions). `.tf` files themselves are editable as part of a planned change after `engage` — OpenTofu reads the same HCL syntax as Terraform.

## Environments

| Name          | Runs On                 | Branch      | Tenant                             |
| ------------- | ----------------------- | ----------- | ---------------------------------- |
| `development` | Local machine only      | `feature/*` | Dev (Entra via local config files) |
| `dev`         | Azure dev resources     | `dev`       | Dev tenant                         |
| `uat`         | Azure Production tenant | `main`      | Production tenant                  |
| `prod`        | Azure Production tenant | `prod`      | Production tenant                  |

- `development` is the local environment name. It is not a cloud environment.
- Local apps use Entra ID for authentication via local configuration files.
- Cloud environments (`dev`, `uat`, `prod`) use App Service environment variables.

## Defaults

Long-lived policy defaults for applications that reuse this skeleton (Entra → Seeding, per the repository agent instructions' Infrastructure work mode). Every default is still shown for confirm-or-override on each environment pass, never silently supplied. Confirmed tenant IDs, subscription IDs, locations, selected SKUs, and other per-instantiation values belong in `manifest.md`, not here.

### Corporate

(single value, no tier variation)

None currently.

### SQL Database — Production Minimum Floor

Every `prod`-tier Azure SQL Database built from this skeleton must meet a documented minimum floor — confirmed against two real Strongwell precedents (a reference production app's applied SKU, and the `maintenance` app's hand-tuned Backups blade) rather than assumed from a single example. `dev`/`uat` are exempt and stay on `infra/modules/variables.tf`'s own baseline defaults. This floor also serves as the Default for Entra → Seeding — dev/uat have no Default here; they stay on that same baseline, unrepresented in this table.

| Setting | Minimum | Terraform variable |
|---|---|---|
| SKU | `GP_S_Gen5_2` | `sql_sku_name` |
| PITR (point-in-time restore) | `31` days | `sql_pitr_days` |
| LTR (long-term retention) | enabled — weekly ≥ `13` weeks, monthly ≥ `6` months, yearly ≥ `2` years (week 1) | `enable_sql_ltr`, `sql_ltr_weekly_retention`, `sql_ltr_monthly_retention`, `sql_ltr_yearly_retention` |
| Backup storage redundancy | `GeoZone` | `sql_storage_account_type` |

These are floors, not fixed answers — a given app may need more, never less. Deliberately not paired with a live cross-region replica or auto-failover group: these apps don't need continuous availability during a regional outage (a day of downtime is acceptable), only for their backups to survive one. `GeoZone` gives geo- and zone-redundant backup storage without a second running database.

This rule governs this repo only — it doesn't reach into `maintenance`'s or the reference app's own repos, even where their real Azure configuration informed the floor above.

`app_name`, `sql_db_name`, the application resource inventory, and all other per-application choices belong in `manifest.md`.

## Naming Conventions

Variables: `<app>` = `works` (lowercase kebab-case), `<db>` = `Works` (PascalCase — SQL database naming follows a different casing convention), `<env>` = `dev`/`uat`/`prod`, and `<hash>` = `substr(sha256(lower(trimspace("<subscription_id>"))), 0, 8)` — a one-way hash of the real subscription GUID, truncated to 8 lowercase hexadecimal characters. `<storage-app>` is `<app>` with hyphens removed, and `<storage-hash>` is the longest leading prefix of `<hash>` that makes the complete Storage Account name fit Azure's 24-character limit, with an allowed length of 6–8 characters.

`<org>` (`stwl`, Strongwell) and `<suffix>` (instance number, `01`) are retired — see Retired Fields below.

| Resource | Real scope | Pattern | Example (dev) |
|---|---|---|---|
| Resource Group | Subscription | `rg-app-<app>-<env>` | `rg-app-works-dev` |
| App Service Plan | Resource Group | `asp-<app>-<env>` | `asp-works-dev` |
| Web App (API) | Global (`*.azurewebsites.net`) | `wa-<app>-api-<env>-<hash>` | `wa-works-api-dev-52796652` |
| Static Web App (SPA) | Global (`*.azurestaticapps.net`) | `swa-<app>-spa-<env>-<hash>` | `swa-works-spa-dev-52796652` |
| SQL Server | Global (`*.database.windows.net`) | `sql-<app>-<env>-<hash>` | `sql-works-dev-52796652` |
| SQL Database | SQL Server | `<db>` | `Works` |
| Storage Account | Global | `<storage-app><env><storage-hash>` — no separators | `worksdev52796652` |

- Only the four globally-scoped resources (Web App, Static Web App, SQL Server, Storage Account) carry `<hash>` — Resource Group, SQL Database, and App Service Plan were never at risk of a cross-tenant collision, so their patterns are unchanged. App Service Plan additionally drops `<org>`, which it never needed either (resource-group-scoped, not subscription- or globally-scoped) — a correction resolved alongside this change, not caused by it.
- Storage Accounts omit all separators and use lowercase alphanumeric only — an Azure platform requirement (`3-24` chars, `^[a-z0-9]+$`), not a stylistic choice. They also omit a resource-type prefix: preserving the recognizable application token is more valuable within this tight limit. Hyphens are removed from `<app>` deterministically rather than replaced with another character.
- The normalized `<storage-app>` must be no longer than 14 characters. Storage uses all 8 hash characters when they fit and shortens only the hash when necessary, never below 6 characters. Invalid input must fail validation before Azure is contacted; the template must never silently truncate the application token.
- This rule is authoritative. `infra/modules/main.tf` must conform to it — if they diverge, the `.tf` file is wrong, not this table. Checked via the Validate step (see the repository agent instructions' Infrastructure work mode).

### Mechanism

`<hash>` = `sha256(lower(trimspace(subscription_id)))`, truncated to 8 hexadecimal characters. The exact normalization is load-bearing: harmless casing or surrounding whitespace in a supplied GUID must not change resource names.

- **The hash input is the subscription GUID only.** `<app>` and `<env>` already appear explicitly in each complete resource name and provide their respective distinctions. Applications and environments in the same subscription therefore reuse the same hash intentionally. If environments later move to separate subscriptions, their suffixes change because the subscription input changes; the formula does not.
- **The suffix is deterministic and does not embed the subscription GUID.** A `random_id`/`random_uuid` Terraform resource was considered and rejected: it would add a new provider dependency, and it isn't disaster-recovery-safe — the value is only stable because it is persisted in state, so lost-and-recreated state produces a different value and can break external references such as DNS and app-registration reply URLs. The hash is a pure function of an already-existing input and can be reproduced after total state loss.
- **Storage Account hash length is deterministic.** `<storage-hash>` takes the first `min(8, 24 - length(<storage-app>) - length(<env>))` characters of `<hash>`. Validation must reject any value that leaves fewer than 6 hash characters. With `prod`, normalized application tokens of 12, 13, and 14 characters therefore use 8, 7, and 6 hash characters respectively.

### Retired Fields

- **`<suffix>` (instance number, `01`).** Never about collision-avoidance — it reserved room for a hypothetical second Web App instance (blue/green, parallel deployment) that was never built. The hash makes it unnecessary for uniqueness; a real disambiguator gets added only if/when an actual second instance is needed, not pre-emptively.
- **`<org>` (`stwl`, Strongwell).** Its only real purpose was global-uniqueness padding, not multi-company/business-unit differentiation — fully superseded by the hash. It was never part of the Resource Group pattern, and its presence in App Service Plan's pattern was already unnecessary before this change (ASP is resource-group-scoped) — that cleanup isn't caused by the hash, just resolved alongside it.

### Exceptions

Deviations from the patterns above, forced by a real constraint (Azure naming collision, platform character/length limit, etc.) rather than a stylistic choice. Record each one in `manifest.md` with the resource, environment, actual name used, and reason — a ratified per-instantiation decision, not something to reverse-engineer later.

There is no automatic collision fallback. If Azure rejects a convention-derived name, OpenTofu must abort. The user then chooses a replacement, records its literal value and rationale in `manifest.md`, has the relevant `.tf` assignment updated to match, and reruns OpenTofu. The template must not invent a suffix, silently alter the name, or retry with a random value.

## Off-Tenant Scaffolding Test (`offtenant`)

A way to validate a `uat`- or `prod`-shaped `infra/<env>` config against the dev tenant — where real hands-on testing is actually permitted — before handing the real config to whoever applies it against its real tenant. Triggered only by the `offtenant` word; see the repository agent instructions' Approval Gates for the full trigger behavior (the state-existence wall, the walkthrough, the cleanup-vs-remind rule). This section is the mechanics; that section is the protocol.

### Mechanics

1. Rename `infra/<env>/main.tf` to `infra/<env>/main.tf.bak` — the real config, left untouched.
2. Write a new `infra/<env>/main.tf`: identical to the backed-up version, except `tenant_id`, `subscription_id`, and `location` are overridden to dev's real values. Everything else — `env` (and therefore resource naming), SKU, backup policy — stays exactly as `<env>` really is; that's the entire point of the test. Start the file with a loud comment block: this is a temporary off-tenant validation copy, the real config is in `main.tf.bak`, restore before any real `<env>` apply.
3. Every `tofu` command against this directory uses an explicit `-state=validate.tfstate` flag — never the default state filename — so it's structurally impossible for this run's state to be mistaken for, or bleed into, the real environment's state later.
4. Command sequence (run by the user — the agent never runs `tofu init`/`plan`/`apply`/`destroy` itself, in any tenant, no exception):
   - `tofu -chdir=infra/<env> init`
   - `tofu -chdir=infra/<env> plan -state=validate.tfstate`
   - `tofu -chdir=infra/<env> apply -state=validate.tfstate`
   - Inspect the result directly in the dev tenant (the agent can also help here via read-only `az`, since this now lives in dev)
   - `tofu -chdir=infra/<env> destroy -state=validate.tfstate`
5. Cleanup: delete `validate.tfstate`, delete the temporary `main.tf`, rename `main.tf.bak` back to `main.tf`. Per the repository agent instructions, the agent does this itself once it has independently verified the resources are gone; otherwise it stops and asks.

### Manual recovery (no agent required)

If a session ends mid-test — the agent is unavailable, context is lost, or anything else interrupts it — the state is always readable directly from the filesystem: if `infra/<env>/main.tf.bak` exists alongside `main.tf`, a test was in progress.

- Check whether `validate.tfstate` still exists in that directory.
  - If it does, use it to check whether the dev-tenant resources are still up (`tofu -chdir=infra/<env> show -state=validate.tfstate`, or just check the Azure portal) — destroy them first if so (`tofu -chdir=infra/<env> destroy -state=validate.tfstate`), then delete `validate.tfstate`.
  - If `validate.tfstate` is already gone, the resources should already be destroyed — but verify in the portal before proceeding, since an empty state file doesn't guarantee an empty resource group.
- Once resolved, delete the temporary `main.tf` and rename `main.tf.bak` back to `main.tf`.

### Precondition

Never begin this procedure if `infra/<env>/terraform.tfstate` (the real state) or a leftover `validate.tfstate` already exists — see the repository agent instructions' Approval Gates for the required agent behavior in that case.
