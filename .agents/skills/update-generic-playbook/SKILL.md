---
name: update-generic-playbook
description: Brainstorm, validate, create, or revise the human-authored generic playbook under playbook/. Use when the human wants to develop generic agent policy or supporting playbook files. Never update an agent-specific playbook.
---

# Update Generic Playbook

Develop the generic playbook as human-authored intent that agent-specific adapters can translate into their native forms.

## Boundaries

- Work only within the confirmed repository root.
- Treat every file under `playbook/` as authoritative generic input.
- Modify only individually approved files under `playbook/`.
- Never modify `AGENTS.md`, `CLAUDE.md`, `agent-playbooks/`, agent adapter skills, or another agent-specific file.
- Follow `playbook/agent-playbook.md` and every supporting policy it references. If those instructions conflict or are unclear, surface the issue before planning changes.

## Develop the Policy

1. Read the complete `playbook/` tree and inspect the local working tree with read-only operations.
2. Disclose existing dirty files and distinguish them from proposed work.
3. Brainstorm and validate the requested policy with the human. Identify conflicts, gaps, unclear terms, structural consequences, and interactions with existing rules.
4. Ask about every uncertainty. Present related questions together unless the human indicates a desire to `iterate`; during that question scope, proceed one question at a time.
5. Do not treat tentative preferences as decisions. Summarize proposed decisions and ask whether the human wants to adopt them and continue.
6. Preserve ratified intent while keeping generic guidance platform-neutral. Let adapters decide how best to express that intent for their agents.
7. Keep `playbook/agent-playbook.md` as the canonical information map. Add, remove, or rename supporting-file references whenever the generic structure changes.

## Obtain Approval

Before changing repository content, present a concise plan that identifies the goal, repository root, every affected file individually, intended effects, material risks, and recommended verification.

Do not create or mutate repository content until the human explicitly replies `go`. Discussion approval is not implementation approval. Apply `go` only to the latest presented plan.

If implementation reveals another affected file, dependency, conflict, or intended change, stop and request a new `go` for a revised plan.

## Implement and Verify

Implement the complete approved generic change without crossing the ownership boundary. Do not stage, commit, restore, clean, or discard repository changes.

After implementation:

- Re-read the complete generic playbook.
- Check internal consistency, canonical structure, direct supporting-file references, and link validity.
- Verify that only approved generic files were changed by this skill.
- Distinguish this skill's changes from files that were already dirty.
- Report verification results and unresolved issues.
- State that agent-specific playbooks may now be stale and must be refreshed separately with the appropriate agent adapter.
