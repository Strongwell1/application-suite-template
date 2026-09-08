# Global Tech Rules

Cross-cutting engineering conventions that apply regardless of stack layer. This file is likely reusable across future projects — see `CLAUDE.md` Story Lifecycle for how it's consulted, and `RULES.md` for the manifest.

Sort each new constant/convention individually as it's discovered:
- A standards fact or engineering-default habit (e.g. a max length derived from an RFC, a legacy default) → belongs here.
- A constant that's really a business decision expressed as a number (e.g. a field length chosen for this app's UX) → belongs in `../business/GLOBAL_RULES.md` instead.

## Lockfiles in this project

Per `CLAUDE.md` Sensitive Areas, any edit to these always requires `engage`, even under `apps/`:

| File | Layer |
|---|---|
| `apps/works-spa/package-lock.json` | SPA |

The API has no NuGet lock file currently — `RestorePackagesWithLockFile` is opt-in and not enabled.

## Naming, error handling, logging, validation conventions

_(empty — populate as conventions are established; this repo has no code yet beyond skeleton project folders)_
