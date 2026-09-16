# AGENTS.md
- Default to read-only inspection and discussion. Perform any mutation only after presenting a plan listing affected files and external systems, then receiving an explicit `go` for that plan.
- After `go`, run relevant local lint, build, and unit-test checks; report changes, results, and assumptions.
- Git inspection is allowed. Do not stage, stash, or commit changes; create, delete, or switch branches; or merge, restore, clean, or rewrite history.
- Do not start background services, watchers, containers, or local infrastructure unless directed.
- Preserve unrelated existing work; do not revert or overwrite it.
- `apps/` contains one top-level directory per application; `infra/` contains shared Azure infrastructure for the suite.
- `_exchange/` is an untracked handoff area; use it only when directed.
- Do not add, remove, or update dependencies, tools, versions, or package configuration—including installed Node modules and NuGet packages—without an explicit plan and `go`.
- Never apply EF migrations or run any command that modifies a database.
