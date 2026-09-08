# works
Strongwell Works Suite - Vertically Integrated Manufacturing Suite to compliment NetSuite

## Repository Structure

| Folder | Purpose |
|---|---|
| `apps/` | Application code — `works-api`, `works-spa`. |
| `docs/` | A place to gather relevant documentation without any managed format — totally user-maintained, but git-tracked. |
| `_dropzone/` | Gitignored, ephemeral staging area for external/temporary reference material the user drops in for context. Claude ignores it unless directed to look. |
| `infra/` | OpenTofu (Terraform-compatible) infrastructure-as-code, one config per environment. |
| `profile/` | This app's concrete instantiation of the patterns defined in `rules/` — real per-environment values. |
| `rules/` | Durable technical, business, and security rules — the transportable conventions this app follows. |
| `stories/` | Business-intent records. `stories/drafts/` holds unratified proto-stories — git-tracked, but Claude ignores it unless directed to look, same spirit as `_dropzone/`. |
| `tasks/` | Engineering execution records for every tracked unit of work. |
