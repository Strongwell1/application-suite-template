# Application Suite Template

A repository template for developing and maintaining an application suite.

## Repository Structure

| Folder | Purpose |
| --- | --- |
| `_exchange/` | Gitignored handoff area for temporary external material. Agents ignore it unless explicitly directed to use it. |
| `apps/` | Application code, with one top-level directory per application. |
| `changes/` | Pending change specifications, a summary log, and an archive of completed changes. |
| `docs/` | Unstructured, human-maintained reference material. Agents ignore it unless explicitly directed to use it. |
| `infra/` | OpenTofu infrastructure-as-code for the suite's cloud resources, organized around the `dev`, `uat`, and `prod` environments. |
| `spec/` | Authoritative source of truth for all application-suite decisions. Its internal structure may evolve. |
