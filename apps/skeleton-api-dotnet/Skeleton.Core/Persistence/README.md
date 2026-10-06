# Entity Framework CLI quick reference

## Read this before running EF commands

- **All Entity Framework CLI commands must be run from the solution root (where `Skeleton.slnx` lives) for these commands to work as shown below.**
- **Before running any Entity Framework CLI command, verify that `Skeleton.Api/efConnectionStrings.json` has the correct `Environment` value set.**
- **The `Environment` value determines which database will be targeted by the CLI command. Running a command with the wrong environment selected can update or revert the wrong database.**
- **Do not point to `dev`, `uat`, or `prod` unless you intentionally mean to apply changes to that shared environment.**
- Replace bracketed values like `[MigrationName]`, `[TargetMigration]`, `[EarliestMigrationName]`, and `[LatestMigrationName]` with real values.
- Keep migration names descriptive and focused on the schema change.
- Migrations are generated into `Persistence/Migrations`.

## development CLI environment targeting

When Entity Framework migrations are run locally from the CLI, the API project uses:

    Skeleton.Api/efConnectionStrings.json

Illustrative example only:

    {
      "Environment": "dev",
      "ConnectionStrings": {
        "development": "<developer-workstation-connection-string>",
        "dev": "<azure-dev-connection-string>",
        "uat": "<azure-uat-connection-string>",
        "prod": "<azure-production-connection-string>"
      }
    }

### How it works

- The `Environment` property selects which connection string will be used.
- If `Environment` is set to `development`, CLI commands will target the developer workstation database.
- If `Environment` is set to `dev`, `uat`, or `prod`, CLI commands will target that corresponding environment's database.
- Always confirm the `Environment` value before running `add`, `update`, `remove`, or `script` commands.

## Context and project settings

These commands use:

- **DbContext:** `AppDbContext`
- **Project:** `Skeleton.Core`
- **Startup project:** `Skeleton.Api`
- **Migrations output folder:** `Persistence/Migrations`

## Common commands

### Add EF migration

Requires a migration name.

    dotnet ef migrations add [MigrationName] --context AppDbContext --project Skeleton.Core --startup-project Skeleton.Api --output-dir Persistence/Migrations

Example:

    dotnet ef migrations add AddWidgetDescription --context AppDbContext --project Skeleton.Core --startup-project Skeleton.Api --output-dir Persistence/Migrations

### Update the database

Applies pending migrations to the database selected by the current `Environment` value in `Skeleton.Api/efConnectionStrings.json`.

    dotnet ef database update --context AppDbContext --project Skeleton.Core --startup-project Skeleton.Api

### Revert the database

Reverts or advances the database to a target migration.

Command format:

    dotnet ef database update [TargetMigration] --context AppDbContext --project Skeleton.Core --startup-project Skeleton.Api

Revert all migrations:

    dotnet ef database update 0 --context AppDbContext --project Skeleton.Core --startup-project Skeleton.Api

Revert to a specific migration:

    dotnet ef database update Initialization --context AppDbContext --project Skeleton.Core --startup-project Skeleton.Api

Parameter notes:

- `[TargetMigration]` is the migration to update the database to.
- Use `0` as the `[TargetMigration]` value to revert the database to an uninitialized state, before any migrations were applied.
- Use a migration name such as `Initialization` to revert or advance the database to a specific migration state.
- `--context AppDbContext` tells EF which DbContext to use.
- `--project Skeleton.Core` points to the project where the DbContext and migrations live.
- `--startup-project Skeleton.Api` points to the startup project used for configuration and application startup.

Important:

- `database update` only changes the database state.
- If you also need to remove the most recent migration file after reverting, use `dotnet ef migrations remove`.
- Reverting the database and removing a migration are different operations.

### Update the dotnet EF tool

    dotnet tool update --global dotnet-ef

### Generate migration script

Generates a SQL script between two migrations.

    dotnet ef migrations script [EarliestMigrationName] [LatestMigrationName] --context AppDbContext

Example:

    dotnet ef migrations script Initialization AddWidgetDescription --context AppDbContext
