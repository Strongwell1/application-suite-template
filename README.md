# works
Strongwell Works Suite - Vertically Integrated Manufacturing Suite to compliment NetSuite

## Repository Structure

| Folder | Purpose |
|---|---|
| `_exchange/` | Gitignored, ephemeral staging area for external/temporary reference material the user drops in for context. The agent ignores it unless directed to look. |
| `apps/` | Application code with a top-level folder per app. |
| `changes/` | Each file represents a change specification. |
| `docs/` | A place to gather relevant documentation without any managed format — totally user-maintained, but git-tracked. |
| `infra/` | OpenTofu (Terraform-compatible) infrastructure-as-code, one config per environment. |
| `rules/` | Durable technical and security rules — the conventions this app follows. |
| `specs/` | Full specification for the application suite. |
