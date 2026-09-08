# CI/CD Tech Rules

Conventions specific to CI/CD deployment identity and pipeline configuration — see `RULES.md` for the manifest.

## GitHub Actions Deployment Identity

### Naming

| Resource | Pattern | Example (dev) |
|---|---|---|
| GitHub app registration | `areg-<app>-github-<env>` | `areg-works-github-dev` |
| Federated credential name | `github-<env>-deploy` | `github-dev-deploy` |

### Federated Credential

Platform/GitHub constants — not project-specific decisions, just the correct values:

| Property | Value |
|---|---|
| Federated credential scenario | `GitHub Actions deploying Azure resources` |
| Issuer | `https://token.actions.githubusercontent.com` |
| Audience | `api://AzureADTokenExchange` |
| Entity type | `Environment` |
| Subject identifier | `repo:<org>/<repo>:environment:<env>` |

Where `<org>` = `Strongwell1` (GitHub org) and `<repo>` = the GitHub repository name (matches `<app>` for Works, but tracked independently — not guaranteed to equal `<app>` for every project).

- Description text pattern: `Allow GitHub to deploy <env> artifacts to Azure tenant`.

### Deployment RBAC

One GitHub deployment identity per environment is granted role assignments directly on each target Azure resource — not `Contributor`, and not scoped to the Resource Group or Subscription:

| Target Resource | Role | Scope |
|---|---|---|
| Static Web App | `Strongwell - Static Web App Deploy Only` | This resource only |
| App Service | `Strongwell - App Service Deploy Only` | This resource only |

Both are pre-existing, tenant-level custom roles (named `Strongwell - ...`, not per-app) — not created per project.

Full least-privilege assessment (permissions, concerns, open questions for security review): see `rules/security/RBAC.md` — status: **Proposed, not yet ratified**.

**Why custom roles instead of a built-in role:** `Contributor` felt too broad for the App Service — it grants far more than "push a deployment" (config, scaling, networking, etc.). The closest built-in role for Static Web App deployment was even more permissive than `Contributor`. Both were rejected in favor of purpose-built, deploy-only custom roles — least privilege by design, not an oversight.

**Critical:** after creating the app registration and federated credential, the RBAC role assignment on the target resource is a separate, easy-to-miss manual step — it isn't granted by the federated credential itself. When adding the role assignment in the portal, search by the app registration's name, not its GUID.

### Exceptions

None recorded yet.
