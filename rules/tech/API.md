# API Tech Rules

Conventions specific to `apps/works-api`. Named `API.md` rather than `BACKEND.md` so a future additional client (e.g. a mobile app) doesn't muddy what "backend" means — see `RULES.md` for the manifest.

## Package manager

- NuGet.

## Migrations

- Local-only migration-file generation: `dotnet ef migrations add <Name>` / `dotnet ef migrations remove`
- Migration-apply command: `dotnet ef database update`

## Generated files

- None currently, beyond standard `bin`/`obj` build output.

## Testing

See `TESTING.md` for the cross-cutting standard (triage, traceability, exclusions).

- Framework: xUnit. Not yet installed — arrives via Dependency-Addition Request when the first test project is created.
- Layout: one test project per production project, created on demand and added to `Works.slnx`: `Works.Core.Tests` first (domain rules live in `Works.Core`), `Works.Api.Tests` if API-layer logic ever warrants it.
- Test naming: `Method_Scenario_ExpectedOutcome`.
- Run: `dotnet test` from `apps/works-api/`.

## Entity & Persistence Conventions

### Primary key

- Every persisted entity has a `Guid Id` — client-assignable (supports offline creation before a sync round-trip), never the clustered index.

### Clustered index column

- Every entity also has a second column: always `bigint`, always a real SQL Server `IDENTITY()`, always configured as the table's clustered index (`HasKey(x => x.Id).IsClustered(false)` plus a separate clustered index on this column). Type and mechanism never vary by entity — only the column's *name* does, decided by a human per entity based on intent, and always exactly one of the two below. Never both on the same entity.
- **`ArrivalSequence`** — fixed name, used when the column has no business meaning. Exists so inserts stay cheap (append-friendly clustering instead of clustering on a random `Guid`) and so unsorted query results still read in roughly chronological order.
- **`ReferenceNumber`** — used when the value is a genuine human-readable, user-facing identifier (a PO number, work order number, etc.). Implemented via a shared `IHasReferenceNumber { long ReferenceNumber { get; } }` interface: the C# member name is always `ReferenceNumber`, but the actual SQL column is mapped to a domain-specific name via `.HasColumnName(...)` (e.g. `PurchaseOrderNumber`, `WorkOrderNumber`) — this keeps joins and flattened reports across tables from colliding on one generic column name. The UI-facing label is a third, independent layer (whatever the business calls it); none of the three names need to match.

### `ReferenceNumber` assignment

- `ReferenceNumber` must never be backed by `IDENTITY()` or a native `SEQUENCE` object — both are documented as not gap-free, since neither participates in transaction rollback (a rolled-back insert still burns the value).
- Instead, a shared control table, `ReferenceNumberControl(EntityName, NextValue)` (singular table name, per this project's table-naming convention), hands out the next value via an ordinary transactional row update (`UPDATE ... SET NextValue = NextValue + 1 OUTPUT ...`), performed inside the same transaction as the entity's own insert — an ordinary DML update rolls back correctly with the rest of the transaction, so a failed insert un-reserves the number instead of burning it.
- Assign the number as late as possible in the transaction (immediately before the insert) to minimize how long the row lock on `ReferenceNumberControl` is held. Some serialization per entity name is inherent to the gapless guarantee itself, not a flaw to engineer around.
- One shared table is the default, not one per entity — row-level locking already isolates contention by `EntityName`. Revisit only if real evidence of same-entity contention appears.
- No period-based reset (per year/fiscal period) is built in by default; the table can be extended to a composite key without data loss if a future entity genuinely needs one.

### Audit and soft-delete

- Implemented via composable interfaces (e.g. `IAuditableCreated`, `ISoftDeletable`), never a shared base class — this lets each entity mix only the traits it actually needs.
- Every entity maintained through the application gets full audit — including lookup/reference tables that might otherwise seem to need less — since a real user is performing the action once it's routed through the app. There is no size-based exception.

### Shared value-object EF configuration

- Any EF configuration for a shared value object (e.g. `Actor`) must go through a shared helper/extension method — never hand-copied per entity's own configuration class. This is what prevents drift (e.g. a `MaxLength` mismatch for the same value object across different entities), independent of how audit/soft-delete traits are modeled.

### Shared constants

- Field-size/length defaults (`MaxEmailLength`, `MaxStringLengthDefault`, etc.) live in exactly one class, `ModelConstraints`, referenced by both the domain and persistence layers. No parallel constants class exists for the same values.
