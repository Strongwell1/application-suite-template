# Testing Rules

Cross-cutting testing standard. Together, the rules files and the unit-test suite form the project's living spec: `rules/` records chosen behavior; the suite proves the code implements it. Stack-specific tooling lives in `API.md` and `SPA.md` — this file is the standard both follow.

A task's own Acceptance Criteria (see `CLAUDE.md`'s Task Lifecycle and the `start-task` skill's Task Plan) states *what* must be true; this file's triage decides *how* that gets verified. AC never implies a unit test uniformly — the triage below, including the database-boundary carve-out, still governs.

## The triage question

For any invariant about to be coded, ask: **did someone decide this, or did the design imply it?**

- **Decided** — a business rule, value range, business-chosen constant, or standards-derived limit. It must exist in the appropriate rules file (sorted per `GLOBAL_TECH_RULES.md`: standards fact → tech, business decision → business) **and** carry at least one citing unit test. Rule ⟺ code ⟺ test, no exceptions.
- **Implied** — an implementation invariant nobody chose (helper preconditions, internal contracts). Unit test only; the test is the spec. Never written into `rules/`.

A rule may exist before its code does (spec-first); the citing-test requirement binds once implementing code exists.

## Traceability

- Every rule in a rules file sits under its own heading — the heading is the citable anchor.
- A citing test carries a comment on the line above it: `Rule: business/GLOBAL_RULES.md § <heading text>`. Renaming a rule heading requires updating its citing tests — `grep -r "§ <heading>"` finds them.
- Enforcement runs one direction: every implemented rule has ≥ 1 citing test; a test needs no rule.

## What not to unit test

CRUD plumbing, framework behavior, generated files, and trivial mappings. "Testable" ≠ "worth testing."

## When integration beats unit

Behavior at the database boundary (EF queries, migrations, transactions) is proven by integration tests, not unit tests against mocked contexts. Integration-test infrastructure is a future decision — until then, database-boundary rules are flagged as untested-by-suite at Review Gate rather than papered over with mocks.
