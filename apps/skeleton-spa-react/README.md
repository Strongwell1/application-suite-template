# Complete Skeleton React SPA

A reusable React 19 SPA template for internal line-of-business applications hosted by Azure Static Web Apps. It uses Vite, TypeScript, TanStack Router/Query/Form, MSAL, Zod, Bootstrap, and npm. The Widget feature is a generic vertical slice intended to be renamed or replaced.

## Start a new application

1. Rename `skeleton` and all user-facing Skeleton branding to the real application name.
2. Rename the Entra application roles in `src/auth/roles.ts` together with the API roles.
3. Rename or replace the Widget slice with the first real domain feature.
4. Replace every environment placeholder.
5. Run formatting, linting, unit tests, and every required environment build.

## Prerequisites

- Node.js 24.x
- npm (use the committed `package-lock.json`)
- A Microsoft Entra SPA registration with delegated access to the API
- The corresponding Skeleton API configuration

The committed environment files are non-secret templates. The application is not expected to authenticate or call Azure successfully until their placeholders are replaced.
`.env.test` contains fixed non-secret values so tests that import the validated API client
remain deterministic.

## Architecture

Feature data flows through explicit layers:

```text
route -> page -> React Query hook -> typed API function -> apiFetch
```

- `src/routes` maps URLs to pages. `src/routeTree.gen.ts` is generated; never edit it manually.
- `src/pages` composes feature screens.
- `src/hooks` owns React Query behavior and cache invalidation.
- `src/api` owns handwritten HTTP contracts and requests.
- `src/auth` owns MSAL, claims, and role helpers.
- `src/components` contains reusable layout, form, feedback, and UI pieces.
- `src/utils` contains pure, unit-tested formatting and mapping helpers.

Keep server state in React Query. Do not add a global state library without a demonstrated need.

## Environments

Vite modes map directly to application environments:

| Command | Mode/file | Environment |
| --- | --- | --- |
| `npm start` | `.env.development` | local `development` |
| `npm run start:dev` | `.env.dev` | Azure `dev` |
| `npm run start:uat` | `.env.uat` | Azure `uat` |
| `npm run start:prod` | `.env.prod` | Azure `prod` |

Hosted environment values are build-time values. Every environment requires:

- `VITE_APP_ENV`
- `VITE_API_BASE_URL`
- `VITE_ENTRA_CLIENT_ID`
- `VITE_ENTRA_AUTHORITY`
- `VITE_API_SCOPE`

`src/env.ts` validates these values at startup and rejects recognizable API/environment mismatches. Vite variables are public client configuration; never put secrets in them.

## Common commands

```bash
npm ci
npm start
npm run format:check
npm run lint
npm test -- --run
npm run build:dev
npm run build:uat
npm run build:prod
npm audit
```

Builds generate the TanStack route tree before TypeScript validation. Commit the generated route tree in the derived repository so editors and clean builds have route types available.

## Authentication and authorization

- MSAL obtains an access token for `VITE_API_SCOPE`.
- `AuthGate` handles startup, authentication errors, and users without basic application access.
- Use `RequireRole` for protected route content and role helpers for conditional controls.
- Never compare role strings ad hoc.
- Client checks are user-experience controls only; the API must enforce every authorization rule.

## Bootstrap conventions

- Bootstrap 5.3 supplies layout, forms, tables, and utilities.
- Prefer semantic HTML and native browser behavior.
- Keep repeated markup and class combinations in shared React components.
- Use application CSS and Bootstrap custom properties for branding; do not patch dependency source.
- Do not add Bootstrap JavaScript unless a specific interactive component requires it.

## API contracts

API contracts are handwritten because this template does not require client generation. Keep TypeScript DTOs aligned with API request/response records. Shared frontend constraints belong beside their domain API types, and contract changes must update both applications in the same work item.

`apiFetch` adds bearer tokens, preserves `FormData` content headers, supports JSON and blob responses, and converts JSON/Problem Details failures to `ApiError`. Correlation IDs from failed API requests are preserved and shown as support references. Forms can map backend Problem Details validation messages back to individual fields.

## Testing

This version intentionally includes unit tests only. Use Vitest for pure utilities, role rules, validation helpers, and isolated reusable components where behavior warrants it. Avoid snapshots and tests of Bootstrap styling. Integration and browser testing may be added by a later template version when the maintenance cost is accepted.

## Adding a feature

Use Widgets as the reference:

1. Add handwritten contracts and endpoint functions in `src/api`.
2. Add query keys and one hook per operation.
3. Add route-level pages and reusable components.
4. Add file-based routes; use a trailing underscore for non-nested paths such as `$widgetId_.edit.tsx`.
5. Apply route and control-level role checks.
6. Add focused unit tests for new pure behavior.

Do not bypass these layers by fetching directly from a page or route.
