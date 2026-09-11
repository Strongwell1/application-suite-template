# Claude Code Playbook Skills Handoff

## Purpose of This Handoff

You are Claude Code working inside the repository whose top-level folder is expected to be `/repos/application-suite-template`.

The human wants you to implement the Claude Code equivalents of two existing Codex project skills:

1. A skill that helps maintain the human-authored generic playbook.
2. A Claude-specific adapter skill that translates the generic playbook into Claude's own playbook files.

This document records the required design and workflow. It is an implementation handoff, not permission to change files immediately. Inspect the live repository first, discuss every uncertainty, present an exact file-by-file implementation plan, and wait until the human replies with the exact command word `go` before creating or modifying repository content.

## Live Sources Take Precedence

Do not rely only on this handoff. Before planning, use read-only inspection to read:

- Every file under `playbook/`.
- `playbook/agent-playbook.md` first, including every supporting policy it directly identifies.
- `.agents/skills/update-generic-playbook/SKILL.md`.
- `.agents/skills/adapt-codex-playbook/SKILL.md`.
- The current root `CLAUDE.md`, if present.
- Every file under `agent-playbooks/claude/`, including files not referenced from `CLAUDE.md`.
- The local working-tree status.

The generic files under `playbook/` are authoritative for intent. The Codex skills are implementation exemplars showing how that intent was translated into skill workflows. This handoff supplies historical design context. If these sources disagree, stop and ask the human rather than silently choosing one.

Do not inspect or follow symbolic links. Symlinks were rejected for this playbook and skill system because they obscure ownership and are harder for developers to follow.

## Confirm the Workspace Boundary

Use read-only local inspection to confirm that:

- The current top-level workspace folder is `/repos/application-suite-template`.
- The local Git root is the same folder.

Anything outside that top-level folder is off-limits for project inspection and mutation. If the starting folder and Git root differ, or if you discover a nested repository or worktree, treat it as a likely layout mistake, explain what you found, and ask the human to confirm the workspace root before continuing.

Normal Claude runtime state in Claude's own home directory and transient files under the guest VM's `/tmp` may be used when appropriate. These runtime exceptions do not expand the project boundary.

## Intended Claude Skill Locations

Use Claude Code's native repository-level skill layout. The intended files are:

- `.claude/skills/update-generic-playbook/SKILL.md`
- `.claude/skills/adapt-claude-playbook/SKILL.md`

Each skill must have its own directory containing its `SKILL.md`. Do not put both `SKILL.md` files directly in `.claude/skills/`, and do not invent a non-native `/claude/skills` location.

Do not add supporting files, scripts, references, metadata, or placeholders unless they are genuinely required by Claude Code's current native skill format. If a platform requirement makes another file necessary, surface that dependency as a plan change and obtain explicit approval before creating it.

Use valid Claude Code skill frontmatter. At minimum, provide a discriminating `name` and `description` consistent with Claude's project-skill format. Keep automatic discovery behavior at its native default unless the human specifically decides otherwise.

## Generic Playbook Architecture

The repository separates generic intent from agent-specific implementation.

### Generic authority

Every file under `playbook/` is human-authored, authoritative generic input. Drafts and unrelated notes belong elsewhere.

`playbook/agent-playbook.md` is the primary generic file and canonical information map. It directly references every supporting generic policy. The generic source establishes canonical file names, relative directory structure, and section order.

### Adapted playbooks

Each supported agent has a conventional root playbook plus a supporting directory:

- Codex: `AGENTS.md` and `agent-playbooks/codex/`.
- Claude Code: `CLAUDE.md` and `agent-playbooks/claude/`.

The adapted playbook preserves generic intent and order but is not a verbatim copy. The adapter should express the requirements in the form Claude follows most reliably. Claude-specific clarification and operational detail are welcome when they strengthen faithful execution. The adaptation must never weaken, omit, or contradict generic intent.

The root `CLAUDE.md` remains Claude's complete primary entrypoint. Supporting Claude files mirror generic supporting filenames and relative structure under `agent-playbooks/claude/`. Each supporting Claude file identifies its matching generic source.

Use Claude's most reliable supported mechanism to make all required supporting guidance effective. Depending on Claude Code's actual capabilities, that may mean direct references, native imports, triggered skills, concise inlining of essential requirements, or another supported mechanism. Do not assume Codex's preferred loading mechanism is automatically best for Claude.

If Claude Code has a genuine platform limitation that prevents the canonical mapping, explain the limitation and propose the smallest exception. Do not implement the exception until the human ratifies the decision and later provides `go` for the mutation plan.

## Skill 1: `update-generic-playbook`

Create a Claude-native skill with this intended capability:

> Brainstorm, validate, create, or revise the human-authored generic playbook under `playbook/` when the human wants to develop generic agent policy or supporting playbook files. Never update an agent-specific playbook.

The exact description may be tuned for Claude's discovery behavior, but it must remain narrow and must clearly exclude agent-specific adaptation.

### Ownership boundary

This skill may modify only individually approved files under `playbook/`.

It must never modify:

- `AGENTS.md`.
- `CLAUDE.md`.
- Anything under `agent-playbooks/`.
- Codex or Claude adapter skills.
- Any other agent-specific file.

### Required workflow

The skill should instruct Claude to:

1. Confirm the workspace root and local Git root agree.
2. Read the complete `playbook/` tree.
3. Follow `playbook/agent-playbook.md` and every supporting policy it identifies.
4. Inspect the local working tree with read-only Git operations.
5. Disclose all files already dirty before proposing changes.
6. Distinguish existing human or agent changes from the new work under discussion.
7. Brainstorm and validate policy with the human before implementation.
8. Identify conflicts, gaps, unclear terminology, structural consequences, and interactions with current rules.
9. Ask about every uncertainty instead of silently resolving it.
10. Keep generic policy platform-neutral and let each adapter choose agent-appropriate mechanics.
11. Maintain `playbook/agent-playbook.md` as the canonical information map when supporting files are added, removed, or renamed.
12. Present a plan listing every affected file individually.
13. Wait for the exact command word `go` before any repository creation or mutation.
14. Implement only the latest approved plan.
15. Stop for a revised plan and new `go` if another affected file, dependency, conflict, or intended change is discovered.
16. Re-read and validate the complete generic tree after implementation.
17. Report that agent-specific playbooks may now be stale, without invoking their adapters.

### Discussion behavior

The skill must preserve these human interaction preferences:

- Do not treat exploratory agreement or a tentative preference as a settled decision.
- Summarize each proposed decision and ask whether the human wants to adopt it and continue.
- Ordinary natural language can ratify a discussion decision, but it cannot authorize repository changes.
- Present related questions together by default.
- If the human says `iterate`, ask the current group of questions one at a time.
- `iterate` applies only to the current question scope, not the entire conversation.
- Every uncertainty should be surfaced because recurring ambiguity is expected to improve the playbook over time.

### Completion checks

After an approved generic update, the skill should require Claude to:

- Re-read every generic file.
- Check internal consistency.
- Check the canonical structure and ordering.
- Verify direct supporting-file references.
- Check internal link validity.
- Verify that only the individually approved generic files were changed by the skill.
- Distinguish the skill's changes from files already dirty.
- Report verification results and unresolved issues.
- State that adapted playbooks may be stale and require a separately invoked adapter.

## Skill 2: `adapt-claude-playbook`

Create a Claude-specific skill with this intended capability:

> Create, refresh, or review the Claude playbook derived from the complete generic playbook when the human asks to adapt `playbook/` into `CLAUDE.md` and `agent-playbooks/claude/`. Never update generic or Codex-specific files.

Tune the wording for Claude's native skill discovery if useful, but preserve the explicit ownership boundary.

### Ownership boundary

This adapter may modify only:

- `CLAUDE.md`.
- Individually approved files under `agent-playbooks/claude/`.

It must never modify:

- Any file under `playbook/`.
- `AGENTS.md`.
- Any file under `agent-playbooks/codex/`.
- This skill itself during an adapter run.
- The generic updater skill.
- Another agent's skill or playbook.

The generic playbook defines intent. If the generic source appears wrong, incomplete, contradictory, or in need of improvement, explain the issue and suggest a future generic change. Do not include that generic repair in the Claude adapter's implementation plan.

### Complete inspection requirement

The skill should instruct Claude to:

1. Confirm that the workspace root and Git root agree.
2. Read every file under `playbook/`, beginning with `playbook/agent-playbook.md` and all policies it references.
3. Read the complete current `CLAUDE.md`.
4. Read every file under `agent-playbooks/claude/`, including unreferenced files.
5. Inspect the dirty working tree with read-only Git operations.
6. Disclose every existing dirty file and surface overlaps for human direction.
7. Detect internal generic conflicts.
8. Detect conflicts between generic and Claude-specific guidance.
9. Detect missing adaptations.
10. Detect orphaned Claude files.
11. Detect broken references, gaps, and drift.
12. Ask the human about every uncertainty before proposing implementation.

The human may intentionally leave Claude's adapted playbook stale for days or weeks while refining the generic source. The adapter must run only when explicitly requested. Never auto-refresh because a generic file changed.

### Canonical Claude mapping

Use these mappings unless a genuine Claude platform limitation requires an approved exception:

- `playbook/agent-playbook.md` maps to root `CLAUDE.md`.
- Every supporting generic file maps by the same relative path under `agent-playbooks/claude/`.

For example, the current generic `playbook/security-policy.md` should map to:

- `agent-playbooks/claude/security-policy.md`.

Preserve the generic section order to make human comparison and drift detection easier. Adapt the language rather than copying mechanically. Add Claude-specific operational detail only when it makes Claude more likely to honor the generic intent.

Keep `CLAUDE.md` as the complete primary Claude entrypoint. It must directly identify every required supporting policy through a reliable Claude-appropriate mechanism. If Claude supports a native import mechanism that is more dependable than a prose reference, evaluate it. The required result is that Claude reliably considers the supporting guidance without unnecessarily overloading context.

### Planning and approval

Before changing anything, the adapter must present a concise plan containing:

- The goal.
- The confirmed repository root.
- Every file to create, modify, rename, or delete, listed individually.
- Intended effects.
- Material risks, dependencies, uncertainties, and assumptions.
- Relevant recommended verification.

Do not list only a directory or wildcard. Do not perform repository mutations until the human replies exactly `go`. A `go` authorizes only the latest plan and expires after that plan is complete.

Once `go` is received, implement all listed changes without asking repeatedly. If implementation reveals any unlisted affected file, new dependency, conflict, issue, or intended change, stop and present a revised plan. Wait for another exact `go`.

### Completion checks

After an approved adaptation, the skill should require Claude to:

- Re-read every generic file.
- Re-read `CLAUDE.md` and every file under `agent-playbooks/claude/`.
- Confirm that generic intent and order were preserved.
- Check canonical file mapping.
- Check each supporting file's generic-source notice.
- Check direct references or native imports and link validity.
- Verify that only approved Claude-owned files changed during the adapter run.
- Confirm that no generic or Codex-specific file was modified.
- Distinguish adapter changes from files already dirty.
- Report deviations, limitations, verification outcomes, and unresolved issues.
- Explicitly confirm that the complete generic and Claude-specific playbook sets were inspected.

## Approval Semantics Shared by Both Skills

The exact word `go` is the boundary between planning and repository implementation.

- Read-only inspection, discussion, brainstorming, and validation do not need `go`.
- Natural-language agreement can settle a discussion decision but cannot authorize file creation or mutation.
- Every repository creation or mutation requires a file-by-file plan and exact `go`.
- `go` covers every listed creation and mutation in the latest plan.
- Do not ask for repeated approval for work already listed and approved.
- A newly surfaced change is a plan deviation and requires a revised plan and new `go`.
- Tool or harness execution approvals are separate from playbook approval and do not substitute for `go`.

Expected transient tool work, Claude-owned runtime state, and normal Git-ignored build artifacts resulting from approved actions do not require separate plan entries. Unexpected non-ignored output or a potentially large blast radius must be surfaced before continuing.

## Security Requirements Shared by Both Skills

The live `playbook/security-policy.md` is authoritative. At minimum, preserve the following non-overridable boundaries in both skills' behavior:

- Work only inside the confirmed repository root for project access.
- Do not inspect the host filesystem outside the guest VM.
- Do not inspect another user's home directory or human credential stores.
- Do not probe private LAN, non-public WAN, mapped drives, or permission-protected resources.
- Do not follow symlinks.
- Do not attempt to bypass technical restrictions.
- Use only Claude's own backend and agent-service credentials.
- Never search for, inspect, copy, or use human credentials.
- Public internet resources may be read when relevant.
- Public GitHub and Azure content may be read without authentication.
- Private or authenticated GitHub and Azure access is absolutely prohibited.
- External interactions other than Claude's required backend services must be read-only.
- Do not upload, publish, trigger, send repository data to, or mutate an external service.
- Disable optional telemetry when possible; do not use a tool if it cannot avoid prohibited external mutation.

Human approval, including `go`, cannot override an absolute security prohibition. If requested work crosses one, explain that the action cannot be performed. The human must change the repository or configuration to make a safe path possible.

## Git Responsibilities

Both skills may use read-only local Git operations for inspection and verification.

Claude must leave these actions to the human:

- Staging.
- Committing.
- Pushing.
- Restoring.
- Cleaning.
- Discarding changes.
- Rewriting history.

Always recommend relevant verification. When a human-only action is needed, keep the handoff conversational and give exact commands when useful. Put every command on its own bullet-list line for easy human parsing. If the human later says an expected action is complete, verify the observable local result when permitted rather than assuming it succeeded.

## Current Repository State Is Not an Assumption

At the time this handoff was written, the working tree included:

- A deleted root `agent-playbook.md` caused by moving the generic source.
- An untracked `playbook/` directory.
- An untracked `.agents/` directory containing the two Codex skills.

This is historical context only. Run a fresh read-only Git status before your plan and disclose the actual current dirty files. Do not stage, restore, clean, or otherwise alter existing work.

## Expected Initial Implementation Scope

If live inspection reveals no conflicting platform requirement, the expected plan will create exactly:

- `.claude/skills/update-generic-playbook/SKILL.md`
- `.claude/skills/adapt-claude-playbook/SKILL.md`

Do not create `CLAUDE.md` adaptations merely because you are creating the adapter skill. Building the skills and running the adapter are separate workflows. The human may choose to review and settle the skill implementation before invoking it.

Likewise, do not modify the generic playbook while creating these skills. The generic playbook updater is a tool for a later, explicitly invoked workflow; its creation does not authorize a generic policy change.

## Recommended Implementation Sequence

This is guidance for planning, not prior authorization:

1. Inspect all live sources and the working tree without mutation.
2. Confirm Claude Code's repository-local skill conventions from locally available authoritative documentation or built-in help when possible.
3. Surface every discrepancy or uncertainty to the human.
4. Summarize the proposed design decisions and ask whether the human wants to adopt them.
5. Present a plan listing the two exact skill files and any genuinely required additional file individually.
6. Wait for exact `go`.
7. Create only the approved files.
8. Validate frontmatter, directory naming, scope boundaries, and internal consistency using Claude's native validation facilities when available.
9. Re-read both completed skills.
10. Inspect Git status and confirm only approved paths changed during this implementation.
11. Report results and leave staging, committing, and any later adapter invocation to the human.

## Final Design Principle

The generic playbook owns intent. A platform-specific skill owns the mechanics that make that intent effective for its agent. Faithful adaptation is more important than textual sameness, but adaptation never grants permission to weaken, omit, or contradict the generic authority.
