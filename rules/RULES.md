# Rules Manifest

One line per rule file — so either party can pick the relevant file(s) without opening everything blind. See `CLAUDE.md` Task Start Protocol for how these are cited and extended during Task Plan drafting and Rulemaking.

## Tech

| File | Purpose |
|---|---|
| [tech/GLOBAL_TECH_RULES.md](tech/GLOBAL_TECH_RULES.md) | Cross-cutting engineering conventions and shared constants that are standards facts or engineering-default habits — naming, error handling, logging, lockfile identification. |
| [tech/TESTING.md](tech/TESTING.md) | Cross-cutting testing standard — decided-vs-implied triage, rule→test traceability, what not to test, database-boundary guidance. |
| [tech/API.md](tech/API.md) | Conventions specific to `apps/works-api`. |
| [tech/SPA.md](tech/SPA.md) | Conventions specific to `apps/works-spa`. |
| [tech/INFRA.md](tech/INFRA.md) | Conventions specific to `infra/`, plus environment topology and cloud-provider specifics. |
| [tech/AUTH.md](tech/AUTH.md) | Entra ID app-registration naming, common values, and the role/security-group/email convention. |
| [tech/CICD.md](tech/CICD.md) | CI/CD deployment identity conventions — GitHub Actions federated credentials and deployment RBAC. |
| [tech/PROFILE.md](tech/PROFILE.md) | Governs `/profile` — this app's concrete instantiation of the patterns defined elsewhere in `/rules`. |

## Business

`rules/business/` is the project's **specification of record**, maintained in lieu of a `/spec` folder. Every business rule captured here must be implemented in code and carry at least one citing unit test once implemented — see [tech/TESTING.md](tech/TESTING.md).

| File | Purpose |
|---|---|
| [business/GLOBAL_RULES.md](business/GLOBAL_RULES.md) | Cross-cutting business rules/invariants that apply across every module, including business decisions expressed as numeric constants. |

Business rules are organized by scope tier — project-wide, module, and deeper if ever warranted — decided when the need is real, not pre-planned as a fixed taxonomy. No tier name is pre-assigned below `module` until a real case exists. Each tier's file is created the moment its first rule exists and is never merged into a broader or narrower scope's file regardless of size, the same way a variable in an inner scope shadows an outer one rather than living inside it.

Naming follows a recursive `GLOBAL.md` sentinel per scope: `business/GLOBAL_RULES.md` is project-wide, `business/<MODULE>/GLOBAL.md` covers a whole module, and so on if a deeper tier is ever needed. Lookup and override are lexical-scoping style — the innermost applicable scope wins silently, with no cross-referencing required.

No module files exist yet; they get created on demand. When one is created, add its row here (or a sub-table for that module's deeper tiers).

## Security

`rules/security/` holds security-assurance assessments (RBAC, data classification, audit, secrets). Unlike `tech/`/`business/`, content here is not ratified by default — each file states its own ratification status explicitly, since content may be actively tested before formal company sign-off.

| File | Purpose |
|---|---|
| [security/RBAC.md](security/RBAC.md) | Least-privilege assessment of custom Azure RBAC roles used for GitHub Actions deployment identity and ACS managed-identity access. |
