---
name: adapt-codex-playbook
description: Create, refresh, or review the Codex playbook derived from the complete generic playbook. Use when the human asks to adapt playbook/ into AGENTS.md and agent-playbooks/codex/. Never update generic or Claude-specific files.
---

# Adapt the Codex Playbook

Translate the complete generic playbook into guidance optimized for Codex while preserving its intent, authority, and order.

## Ownership

This skill may modify only:

- `AGENTS.md`
- Individually approved files under `agent-playbooks/codex/`

Never modify:

- Any file under `playbook/`
- `CLAUDE.md`
- Any file under `agent-playbooks/claude/`
- This skill or another agent's skill

The generic playbook defines intent. This skill is the Codex-specific adapter and never repairs generic-source problems itself.

## Inspect Completely

1. Confirm the workspace root and its Git root agree. If they do not, stop and ask the human to confirm the boundary.
2. Read every file under `playbook/`, beginning with `playbook/agent-playbook.md` and every supporting policy it identifies.
3. Read `AGENTS.md` and every file under `agent-playbooks/codex/`, including files not referenced by `AGENTS.md`.
4. Inspect the working tree with read-only operations. Disclose all existing dirty files and surface overlapping changes for human direction.
5. Detect internal generic conflicts, conflicts between generic and Codex guidance, missing adaptations, orphaned Codex files, broken references, gaps, and drift.
6. Ask about every uncertainty before proposing implementation. Follow the generic playbook's grouped-question, `iterate`, and decision-ratification behavior.

If the generic playbook needs improvement, explain the problem and suggest a specific generic change for later consideration. Do not include that change in this adapter's implementation plan.

## Adapt for Codex

Use these canonical mappings:

- `playbook/agent-playbook.md` maps to root `AGENTS.md`.
- Every supporting generic file maps by the same relative path under `agent-playbooks/codex/`.

Preserve generic section order and intent, but do not copy mechanically. Rephrase, clarify, and add Codex-specific operational detail when that will make Codex follow the guidance more reliably. Never weaken or contradict a generic rule.

Make each supporting Codex file identify its matching generic source. Keep `AGENTS.md` as the complete primary Codex entrypoint. Ensure it directly identifies every supporting requirement through the most reliable Codex-appropriate mechanism, such as a clear conditional reference, a triggered skill, or concise inlining of essential rules.

Mirror generic naming and structure. If a genuine Codex platform limitation requires a deviation, surface it and explain the smallest proposed exception before seeking approval.

## Obtain Approval

Before changing repository content, present a concise plan that identifies the goal, repository root, every affected file individually, intended effects, material risks, and recommended verification. Explain only non-obvious adaptation choices, platform limitations, or structural exceptions; the human will review the resulting files directly.

Do not create or mutate repository content until the human explicitly replies `go`. Discussion approval is not implementation approval. Apply `go` only to the latest presented plan.

If implementation reveals another affected file, dependency, conflict, or intended change, stop and request a new `go` for a revised plan.

## Implement and Verify

Implement every approved Codex adaptation without crossing the ownership boundary. Do not stage, commit, restore, clean, or discard repository changes.

After implementation:

- Re-read every generic file and every Codex-specific playbook file.
- Confirm that generic intent and order are preserved.
- Check canonical file mapping, source notices, direct references, and link validity.
- Verify that only approved Codex-owned files were changed by this skill.
- Confirm that no generic or Claude-specific file was modified.
- Distinguish this skill's changes from files that were already dirty.
- Report verification results, deviations, limitations, and unresolved issues.
- Explicitly confirm that the complete generic and Codex-specific playbook sets were inspected.
