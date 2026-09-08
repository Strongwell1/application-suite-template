# Profile Tech Rules

Governs `/profile` — this app's own concrete instantiation of the patterns
defined elsewhere in `/rules`. See `RULES.md` for the manifest.

## Purpose

`/profile` holds decided, per-app values — real tenant/subscription IDs,
real registration names, real GUIDs, which role-naming tier this app
picked — as distinct from `/rules`, which holds transportable patterns any
app built from this skeleton could reuse. See `CLAUDE.md`'s Work Modes —
Infrastructure subsection for how `/profile` fits into the rules→profile→
wiring model.

## Naming

UPPERCASE, matching the rest of `/rules`' file convention — not the
lowercase-kebab convention used by numbered `/tasks`/`/stories` detail
files.

| File | Contents |
|---|---|
| `DEV.md` | dev environment: identity (tenant/subscription/location/SKU), app registrations, GitHub deploy identity |
| `UAT.md` | same shape, uat environment |
| `PROD.md` | same shape, prod environment |
| `ROLES.md` | shared roles/security-groups table — one table, not one per environment; environment shown only as a column where it actually varies (e.g. Security Group Object Id) |

## Creation trigger

A file is created only once real values exist for it. No placeholder files
for an environment that hasn't been provisioned yet — an empty
`UAT.md`/`PROD.md` sitting in the repo implies false completeness. This
mirrors the "no ceremony for its own sake" principle already governing
Entra Seeding.

## Migration discipline

When a fact's home moves (e.g. `ENTRA.md` → `/profile`), add the new home
first, verify the fact landed there correctly, and only then remove the old
home. Never let a fact become unreachable — even briefly, even across a
session boundary. This is Validate→Implement→Verify applied to the
migration itself (see `CLAUDE.md`'s Work Modes — Infrastructure).

## Content template (per environment file)

Mirrors what `ENTRA.md` already proved out:

1. Identity block — Tenant Id, Subscription Id, Location, App Service Plan
   SKU, App Name, SQL Database Name (plus any prod-only floor values, e.g.
   SQL Database SKU/Backup Policy)
2. App Registrations table
3. GitHub Deploy Identity table

## Relationship to `/rules`

Every value here instantiates a pattern defined in `rules/tech/INFRA.md`,
`AUTH.md`, or `CICD.md` — `/profile` never declares a new pattern or
variable itself; that's out of scope here and routes to the relevant
`/rules` file instead (mirrors the existing Structure/Entra scope boundary
in `CLAUDE.md`'s Work Modes — Infrastructure).
