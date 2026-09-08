# Auth Tech Rules

Conventions specific to Entra ID app registrations, roles, and security groups — see `RULES.md` for the manifest.

## Naming Conventions

### Variables

| Variable | Meaning | Value |
|---|---|---|
| `<app>` | Application name | `works` |
| `<module>` | A closely related portion of a larger application, cohesive enough to be refactored into its own application fairly easily | varies — e.g. `Sales`, `Inventory` |
| `<env>` | Environment | `dev` / `uat` / `prod` |
| `<domain>` | Email domain | `strongwell.com` |

### App Registrations

| Resource | Pattern | Example (dev) |
|---|---|---|
| API app registration | `areg-<app>-api-<env>` | `areg-works-api-dev` |
| SPA app registration | `areg-<app>-spa-<env>` | `areg-works-spa-dev` |
| Postman app registration | `areg-<app>-postman-<env>` | `areg-works-postman-dev` |

- Deliberately omits an `<org>`/tenant-code segment (unlike infra resource names) — app registration names only need to be unique within the tenant, not Azure-wide.

### UAT / Production Scope

UAT and Production share a single Azure tenant (the Production tenant — see `rules/tech/INFRA.md`'s Environments table), but are not identical below the tenant level:

- **App registrations, enterprise applications (service principals), and app roles are provisioned separately per environment** — UAT gets its own `areg-<app>-*-uat` registration, distinct Client ID, and its own app-role definitions, distinct from Production's. This is deliberate: standing up UAT's registrations first surfaces tenant misalignments before Production resources are created.
- **Security groups are not provisioned separately.** One set of email-enabled security groups exists in the Production tenant, shared by both UAT's and Production's app roles — each environment's enterprise application carries its own `azuread_app_role_assignment` pointing at the same group. Creating and mail-enabling a second, UAT-only set of groups was judged not worth the ongoing M365 maintenance cost.
- If UAT and Production security ever need to diverge (e.g. different test-vs-real membership), this section and the shared-group assumption must be revisited together — it is not a decision to reverse silently. (No Entra Terraform currently exists to carry this assumption — see Task 0053 — so for now this applies to the assumption itself, to be re-wired into Terraform whenever that work resumes.)

### Common Values (API app registration)

Kept identical across every environment so the Azure portal reads consistently:

| Property | Value |
|---|---|
| Scope name | `access_as_user` |
| Admin consent display name | `access_as_user` |
| Admin consent description | `Allow access to <app> API as signed-in user` |
| User consent display name | `access_as_user` |
| User consent description | `Allow access to <app> API as you` |
| Manifest change | `"requestedAccessTokenVersion": 2` |

- Single scope per API — all real authorization happens via App Roles, not additional scopes. Revisit only if a second, less-trusted client needs to be capped below what a user's role would otherwise allow.

### Role Tiers, Security Groups, and Group Emails

Every app picks exactly one of the two per-app tiers below — the choice is
recorded in `/profile/ROLES.md` (see `rules/tech/PROFILE.md`), not here.
`Global.<role-name>` is a separate, third tier, unaffected by the per-app
choice — shared across every app (e.g. `Global.Developer`).

| Tier | Role | Security Group | Group Email (Optional) |
|---|---|---|---|
| Global (tenant-wide) | `Global.<role-name>` | `App-Global-<role-name>` | *(none)* |
| Flat (per-app) | `<app>.<role-name>` | `App-<app>-<role-name>` | `<app>-<role-name>@<domain>` |
| Module-scoped (per-app) | `<app>.<module>.<role-name>` | `App-<app>-<module>-<role-name>` | `<app>-<module>-<role-name>@<domain>` |

- Role names always use dots. Hyphens are reserved for the security-group/email patterns only.
- `Global.Developer` is reused, not newly created per app.
- **Guaranteed baseline roles, regardless of tier:** every app defines at least `Administrator` (top-level/full permissions) and `User` (default/minimum permissions), under whichever tier it picked.
- Flat suits a typical small LOB app — one `Administrator`/`User` pair, no further breakdown. Module-scoped suits an app with several closely-related-but-separable portions (see `<module>` above) — `works` picked this tier (recorded in `/profile/ROLES.md`).
- Admin authority crossing module boundaries is a property of the flat tier, not a platform-wide law. An app on the module-scoped tier keeps admin authority scoped per module — no suite-wide `<app>.Administrator` spanning modules (this is `works`' case).
- Multi-word module names drop spaces (`ShopFloor`, not `Shop Floor`).

### SPA-Specific Values

| Property | dev | uat / prod |
|---|---|---|
| Platform | `Single-page application` | `Single-page application` |
| Redirect URIs | Local dev server (e.g. `http://localhost:5173`) **and** the deployed SWA URL | Deployed SWA URL only |
| API Permissions | matching-environment API app's `access_as_user` (Delegated) + Microsoft Graph `User.Read` (Delegated) | same |

- Local redirect URI only applies to `dev` — nobody runs local dev against `uat`/`prod` tenants.

### Postman-Specific Values

| Property | Value |
|---|---|
| Platform | `Mobile and desktop applications` |
| Redirect URI | `https://oauth.pstmn.io/v1/callback` (fixed — Postman's own service endpoint, not app- or env-specific) |
| API Permissions | matching-environment API app's `access_as_user` (Delegated) |

- Postman is a developer testing tool, not a first-party client — its registration exists purely to let a developer exercise the API manually via OAuth.

### Environment-Specific Values

- Tenant Id: don't restate here — see `rules/tech/INFRA.md`'s Environments table.
- Application (Client Id): populate per environment only once actually provisioned. Blank means that environment hasn't been created yet — once app registrations move to OpenTofu, Terraform state itself becomes the record of what exists, and this note stops being necessary.

### Exceptions

None recorded yet.
