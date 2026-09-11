# Agent Playbook

## Purpose

This directory is the human-authored, generic source of truth for agent playbooks. It expresses intent and required outcomes without assuming that every agent uses the same mechanisms.

Agents never update their own adapted playbooks merely because a generic file changed. A human must explicitly invoke the appropriate agent adapter. Generic playbook updates and agent-specific adaptations are separate workflows.

This playbook operates within mandatory platform, safety, sandbox, and permission constraints. Approval under this playbook cannot override those constraints. Agents surface any conflict to the human.

## Information Structure

Every file under `playbook/` is authoritative input. Drafts and unrelated notes belong elsewhere.

This file directly references every supporting generic policy:

- [Security Policy](security-policy.md)

The generic playbook establishes the canonical file names, order, and directory structure. Agent-specific playbooks mirror that structure unless a genuine platform limitation requires an exception. An adapter must explain any such exception for human review.

The primary generic file maps to each agent's conventional root file, such as `AGENTS.md` for Codex or `CLAUDE.md` for Claude. Supporting files map by relative name into `agent-playbooks/<agent>/`.

## Adaptation

An adaptation preserves the intent and order of the generic playbook while expressing the guidance in the form best followed by that agent. Verbatim copying is not required. Agents may add platform-specific clarification, examples, and operational detail, but may never weaken or contradict the generic intent.

Each adapted file identifies its matching generic source. The primary agent file directly identifies all required supporting guidance using reliable, agent-appropriate mechanisms. The generic playbook defines the required outcome; the adapter chooses whether its agent is best served by references, native imports, triggered skills, concise inlining, or another supported mechanism.

An adapter must:

- Read the entire generic playbook tree and the invoking agent's entire adapted playbook.
- Update only the invoking agent's conventional root file and its directory under `agent-playbooks/`.
- Never modify the generic playbook or another agent's files.
- Detect missing adaptations, orphaned agent files, broken references, conflicts, gaps, and drift.
- Stop and ask the human about every uncertainty or conflict before proposing implementation.
- Suggest generic improvements when useful, but leave their implementation to the generic playbook workflow.
- Confirm after its run that every generic and applicable agent-specific playbook file was inspected.

The generic playbook update workflow may modify only files under `playbook/`. It never modifies agent-specific files. After a generic update, report that adapted playbooks may be stale; do not invoke adapters automatically.

## Operating Mode

Default to discussion and planning. Read-only inspection is allowed. Do not run mutating commands or create, modify, rename, or delete repository content until the plan is approved.

When exploring, explaining, brainstorming, validating, or reviewing, answer and discuss without implementing.

Ask about every uncertainty rather than making assumptions. Present related questions together by default for efficiency. If the human indicates a desire to `iterate`, ask the current group of questions one at a time and allow each answer to develop into discussion. Related questions discovered during that discussion remain in the current iteration scope. Return to grouped questions after that scope is resolved.

Do not treat a tentative preference or exploratory agreement as a decision. Summarize each proposed decision and ask whether the human wants to adopt it and continue. Ordinary natural language is sufficient to ratify or reject a discussion decision. Ratification never authorizes repository changes.

## Planning and Approval

Before proposing repository changes, inspect the working tree. Disclose every existing dirty file so the human understands the likely commit scope. If existing changes overlap or may affect the proposed work, surface the condition and wait for direction.

Before acting, present a concise plan containing:

- The goal.
- The repository and workspace root.
- Every file to be created, modified, renamed, or deleted, listed individually.
- The intended actions and expected effects.
- Risks, uncertainties, dependencies, and assumptions.
- Relevant recommended verification.

Literal commands are not normally required in a plan. Provide commands when handing off an action that the agent cannot perform and the human must execute.

The human must explicitly reply with the command word `go` before any repository creation or mutation. Other forms of approval do not authorize implementation. A `go` applies only to the most recently presented plan and expires when that plan is complete.

After `go`, perform every approved creation and mutation without seeking further permission. Do not change an unlisted file. Any newly discovered dependency, issue, affected file, or intended change is a plan deviation: stop, surface it, present a revised plan, and wait for a new `go`.

Expected temporary tool output, agent-owned caches, and normal Git-ignored build artifacts produced by an approved action do not require individual approval. If generated output is unexpectedly not ignored or could create a large blast radius, stop and wait for human direction. If an approved action unexpectedly changes an unlisted source-controlled or non-ignored file, leave it untouched, report it, and wait for direction; do not revert it on your own.

## Verification and Reporting

Always recommend verification relevant to the proposed change. After implementation, perform approved verification when possible. Never assume the human completed an expected action; verify observable local state when permitted.

If verification finds a discrepancy, failure, or incomplete human action, surface it and wait for direction. Do not correct it without an approved plan.

The implementation report distinguishes:

- Files changed by the agent.
- Files that were already dirty.
- Verification performed and its outcome.
- Deviations, limitations, and unresolved issues.

## Git Responsibilities

Agents may use read-only local Git operations for inspection and verification. Staging, committing, pushing, restoring, cleaning, discarding work, and rewriting history are human responsibilities. Agents never perform those actions.

## Human Handoffs

Keep handoffs conversational. When progress depends on human review, testing, or a human-only action, explain what is needed in plain language, provide exact commands when useful, and ask the human to report the outcome. Do not require a rigid template or restate limitations already established by the playbook.

Place every command on its own bullet-list line so it is easy for the human to identify and parse.
