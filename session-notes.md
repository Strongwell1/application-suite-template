# Playbook Design Session Notes

## Document Status

This document is a detailed historical record of the design discussion that produced the repository's generic agent playbook and the first Codex playbook-management skills.

It is not an authoritative policy file. The current files under `playbook/` are the generic source of truth, and the installed agent-specific skills and playbooks are the current implementations. If this record conflicts with a live policy or skill, inspect the live files and ask the human how to resolve the discrepancy.

The purpose of retaining this record is to preserve the context, reasoning, terminology, rejected alternatives, and interaction preferences behind the design. A future agent can use it to answer questions about why the system has its present shape even after the original conversation is no longer available.

## Session Objective

The session began as a review of a deliberately spartan `agent-playbook.md`. The human's central idea was that a generic playbook should express intent, while each AI agent should create an adapted version optimized for how that agent best understands and follows instructions.

For Codex, the eventual adapted primary playbook will be `AGENTS.md`. For Claude Code, it will be `CLAUDE.md`. The generic source must not be treated as a mechanically copied template. It is an authoritative statement of intent that an agent-specific adapter translates without weakening or contradicting it.

The discussion expanded into four related designs:

1. The generic playbook's authority, information structure, and approval rules.
2. A separate generic security policy.
3. The layout and responsibilities of agent-specific adapted playbooks.
4. Separate skills for maintaining the generic playbook and adapting it for a particular agent.

## Core Mental Model

The agreed model has three distinct layers.

### Generic source of truth

Everything under `playbook/` is human-authored, generic, authoritative input. It should describe intent and required outcomes without assuming every agent has the same context-loading, file-reference, or skill mechanisms.

The primary generic file is `playbook/agent-playbook.md`. It is also the canonical information map: it directly identifies every supporting generic policy file. At the end of this session, its only supporting policy was `playbook/security-policy.md`.

### Agent-specific adapted playbooks

Each agent has a conventional root playbook and a supporting directory:

- Codex uses root `AGENTS.md` and `agent-playbooks/codex/`.
- Claude Code uses root `CLAUDE.md` and `agent-playbooks/claude/`.

The root agent file remains that agent's primary entrypoint. Supporting files mirror the generic structure and names under the agent's own directory.

The adapted files preserve generic intent and section order, but they do not need to be verbatim. Each adapter may rephrase the guidance, add platform-specific clarification, and choose the mechanism its agent follows most reliably. Examples include direct references, native imports, triggered skills, or concise inlining of essential requirements.

### Maintenance skills

Generic policy maintenance and agent-specific adaptation are intentionally separate functions.

- A generic playbook update skill may change only files under `playbook/`.
- An agent adapter may read the generic playbook but may change only that agent's conventional root playbook and supporting agent directory.
- Neither workflow automatically invokes the other.

The human may spend an extended period refining the generic playbook without refreshing any adapted playbook. Staleness is permitted and should be reported, not automatically repaired.

## Authority and Information Structure Decisions

The following decisions were ratified during discussion:

- The top-level folder is the workspace boundary.
- In this repository, the agreed workspace root is `/repos/application-suite-template`.
- The workspace root should match the local Git root.
- Everything outside that top-level folder is off-limits for project access.
- The generic source belongs under the visible `playbook/` directory.
- Every file under `playbook/` is authoritative generic input.
- Drafts and unrelated working notes must live outside `playbook/`.
- `playbook/agent-playbook.md` directly references every supporting generic file.
- The generic playbook establishes canonical file names, relative structure, and ordering.
- Agent adapters mirror that structure unless a bona fide platform limitation requires an exception.
- Any platform exception must be surfaced and explained before implementation.
- Supporting adapted files identify the generic file from which they were derived.
- Source order should be preserved because stable ordering makes human diffs and drift detection easier.
- The content is adapted rather than copied verbatim; the goal is faithful intent expressed in the form best followed by the target agent.

The human preferred explicit, visible agent directories because a person can immediately tell which agents are in use and compare their implementations. Extra layers of subdirectories were considered unnecessary unless the structure grows enough to justify them.

## Separation of Responsibilities

### Generic playbook updater

The generic updater is a brainstorming, content-validation, and possible content-mutation workflow. It must:

- Read the complete generic playbook tree.
- Inspect the dirty working tree before proposing changes.
- Help the human explore and refine generic intent.
- Detect conflicts, gaps, unclear terms, structural consequences, and interactions with current rules.
- Modify only individually approved files under `playbook/`.
- Maintain direct references from `playbook/agent-playbook.md` whenever supporting structure changes.
- Never modify `AGENTS.md`, `CLAUDE.md`, `agent-playbooks/`, or adapter skills.
- Report that adapted playbooks may be stale after a generic change.
- Never invoke an adapter automatically.

### Agent-specific adapter

Each agent uses its own adapter skill. A single adapter that asks which agent to target was rejected as unnecessarily dangerous because it increases the chance of writing the wrong agent's files.

An adapter must:

- Be hard-coded to its own agent identity and output boundary.
- Read every generic playbook file.
- Read the complete adapted playbook tree for its own agent, including unreferenced files.
- Detect internal generic conflicts, generic-versus-agent conflicts, missing adaptations, orphaned agent files, broken references, gaps, and drift.
- Ask the human about every uncertainty before implementation planning.
- Translate generic intent into the form most reliable for its specific agent.
- Preserve generic order and canonical naming unless a real platform issue requires a surfaced exception.
- Modify only its agent's primary root file and individually approved supporting files.
- Never repair the generic source itself.
- Suggest a generic improvement when appropriate, while leaving it to the generic updater workflow.
- Never modify another agent's files.
- Confirm after completion that it inspected the complete generic and agent-specific sets.

For Codex, the mapping is:

- `playbook/agent-playbook.md` to `AGENTS.md`.
- Each supporting generic file to the same relative name under `agent-playbooks/codex/`.

For Claude Code, the intended parallel mapping is:

- `playbook/agent-playbook.md` to `CLAUDE.md`.
- Each supporting generic file to the same relative name under `agent-playbooks/claude/`.

## Discussion and Decision Behavior

The human emphasized that an agent must not silently treat an exploratory preference as a settled decision. Earlier in the discussion, Codex sometimes assumed a decision had been ratified and moved to the next topic. The preferred interaction is:

1. Discuss a question and possible resolution.
2. Summarize the proposed decision.
3. Ask whether the human wants to adopt that decision and continue.
4. Proceed only after the human ratifies it.

Ordinary natural language is sufficient to approve or reject a discussion decision. This kind of ratification does not authorize repository mutation.

The generic default is to present related questions together because it is efficient for the agent and token usage. The human introduced the command `iterate` for cases where a group of questions is likely to require discussion. When the human says `iterate`:

- It applies only to the current scope of questions.
- The agent asks those questions one at a time.
- Related questions discovered during that discussion remain part of the current iteration scope.
- It does not change the interaction mode for the entire session.
- After the current question scope is resolved, grouped questions may resume.

Every uncertainty should be surfaced. The expectation is that recurring uncertainty will cause the generic playbook to improve over time, reducing ambiguity through attrition.

## Planning and the `go` Boundary

The `go` boundary exists to give the human a clear opportunity to see and approve the full scope of permanent repository changes before an agent shifts from planning to implementation.

The required behavior is:

- Discussion, explanation, brainstorming, validation, and read-only inspection do not need `go`.
- Every workflow that creates or mutates repository content requires a plan followed by the exact command word `go`.
- Other expressions such as "yes," "looks good," or "proceed" may ratify a discussion decision but do not authorize repository mutation.
- The plan must identify the confirmed repository root.
- The plan must list every affected file individually. A directory, wildcard, or broad category is not enough.
- The plan must state intended effects, meaningful risks, dependencies, assumptions, and relevant verification.
- Exact shell commands are generally unnecessary in plans.
- A `go` applies only to the latest presented plan.
- Once `go` is received, every listed creation or mutation is approved and should be completed without repeated permission requests.
- A previously undiscovered dependency, issue, affected file, or intended change is a plan deviation.
- A deviation must be surfaced with a revised plan and requires a new `go`.
- An agent must not continue into a large blast radius merely because an earlier, narrower plan was approved.

The human summarized the philosophy as: it is easier not to do something than to unbreak an egg.

Harness or sandbox command approvals are separate from playbook approval. The playbook's `go` requirement governs repository mutation scope. A tool harness may independently prompt for execution permission, but that prompt does not replace the playbook plan or `go`, and the playbook is not expected to micromanage the harness's command-level UI.

## Dirty Working Tree Behavior

Before proposing a mutation plan, an agent must inspect the local working tree and disclose every existing dirty file. This is needed because those files may become part of the human's next commit scope, even if the agent did not create them.

If existing changes overlap the proposed work, the agent must surface the overlap and wait for human direction. This situation should be exceptional. A likely explanation is that the human deliberately requested multiple related changes within one future commit, but the agent must investigate rather than assume.

If a file was already dirty and the approved plan will modify it, the plan must make that fact clear.

The agent must not restore, clean, discard, stage, or commit dirty work on its own.

## Expected Artifacts and Temporary Work

The approval boundary is aimed at intended, persistent repository changes. It is not meant to block normal transient tool operation.

The following do not require individual plan entries when they are incidental to an approved action:

- Temporary work areas used by tools.
- Agent-owned runtime state and caches.
- Normal Git-ignored build artifacts such as `bin`, `obj`, or `node_modules` when produced by an approved build or test.

If expected generated output is unexpectedly not ignored, affects source-controlled files, or could create a large blast radius, the agent must stop, report the issue, and wait for direction. It must not silently proceed or revert the output on its own.

`/home/agent/*` was explicitly accepted for the agent's own normal runtime and cache use. `/tmp` was also accepted for transient tool files. These are runtime exceptions, not extensions of the project workspace.

## Workspace and Filesystem Security Decisions

The top-level folder terminology was preferred over "top-level file." For this session:

- Top-level workspace folder: `/repos/application-suite-template`.
- Local Git root: expected to be `/repos/application-suite-template`.
- Anything outside that boundary is off-limits for project inspection and mutation.

If the current folder is not the Git root, Git reports another root, or a nested repository or worktree is discovered, the agent should treat it as a likely mistake. The human stated that nested Git repositories should never be created intentionally in this workflow. The agent must surface the condition and confirm the agreed workspace root before proceeding.

Worktrees were distinguished from ordinary nested repositories: a linked Git worktree is a separate checkout tied to a repository, not simply a nested repository. Nevertheless, discovering either an unexpected nested repository or worktree inside the agreed root is anomalous and requires human direction.

Symbolic links were ultimately prohibited for the playbook and skill structure. The initial security rule was not to follow a link outside the repository; the human then chose the simpler team convention of avoiding symlinks altogether because they are harder for other developers to understand and can obscure boundaries.

Agents must never try to inspect or access:

- The host filesystem outside the guest VM.
- Another user's home directory.
- Human credential stores.
- Private LAN resources.
- Non-public WAN resources.
- Mapped or shared drives outside the authorized workspace.
- Resources hidden behind permission boundaries.

The fact that permissions might technically expose something does not imply authorization. The agent must never attempt to bypass filesystem or network restrictions.

If a path or likely content may enter a prohibited area, the agent should notify the human before investigating it. If the action itself is absolutely prohibited, the agent must explain that the human cannot override the rule with `go`; the human must change the repository or environment configuration so the work can continue safely.

## Credentials and Sensitive Content

Agents necessarily access their own model backends and agent-service resources, which require agent-managed credentials. That is expected and permitted.

Agents must never search for, inspect, copy, or use a human's credentials in an attempt to help. The human expects operating-system permissions to block much of this access, but the behavioral prohibition applies even if permissions are accidentally permissive.

The human noted that most Azure usage relies on managed identities and that local testing may use hardcoded test values inside the physically controlled VM. Local hardcoded test values are not automatically prohibited. However, if an agent believes a workspace path or content might expose prohibited credentials or sensitive content, it must stop before further inspection, notify the human, and wait for direction.

If a credential or prohibited value appears accidentally in output, it must not be repeated, quoted, echoed, or logged again.

## External Resource Policy

The final external-resource distinction was between public read-only access and private or mutating access.

Permitted:

- Agent backend/model interactions required for the agent to function.
- Read-only access to publicly available internet resources.
- Read-only access to public GitHub or public Azure documentation and content.

Prohibited:

- Any private or authenticated GitHub access.
- Any private or authenticated Azure access.
- Access to private repositories, tenants, APIs, or services.
- External mutations such as uploading, publishing, triggering, sending data, or changing remote state.
- Using human credentials for any external access.
- Optional telemetry when it can be disabled; tools that cannot avoid a prohibited external write should not be used.

The human first stated "No GitHub or Azure access at all" and then clarified the intended boundary: private access to GitHub and Azure is completely off-limits, while public read-only access is allowed. All Git operations performed by an agent are local and read-only.

## Git Responsibilities

Git staging and history operations are human responsibilities after code or text review and, when appropriate, testing.

Agents may use local, read-only Git commands to inspect or verify state. Agents must not:

- Stage changes.
- Commit changes.
- Push changes.
- Restore or discard files.
- Clean the working tree.
- Rewrite history.

The agent should always recommend relevant verification. Example commands are a bonus when a human must perform an action the agent cannot perform.

When progress depends on a human action, the interaction should remain conversational rather than dogmatically structured. For example, after the human reviews and tests changes and says they look good, the agent may suggest the appropriate Git commands and ask the human to report the outcome. Every command must be placed on its own bullet-list line so it stands out and is easy to parse.

After the human says an expected action is complete, the agent should verify the observable local result when possible. Local Git actions are easy to verify. Remote Git or Azure effects are not available to the agent and, under this security policy, must not be accessed privately at all. If verification shows a discrepancy, the agent surfaces it and waits for direction instead of silently correcting it.

## Verification Expectations

Relevant verification must always be recommended in the plan. After an approved implementation, the agent should perform the approved verification that is possible locally.

An implementation report should distinguish:

- Files changed by the agent.
- Files already dirty before implementation.
- Verification performed and its outcome.
- Deviations, limitations, and unresolved issues.

The agent should consider that the human may not have perfectly followed a handoff. When observable, expected human actions should be checked rather than assumed.

## Directory Structure Discussion

The generic and adapted playbook files were separated so a human can compare them easily:

```text
playbook/
|-- agent-playbook.md
`-- security-policy.md

AGENTS.md
agent-playbooks/
|-- codex/
`-- claude/

CLAUDE.md
```

The intended mapping is structural rather than verbatim. The generic source controls names, relative layout, and order. Each adapter controls the agent-appropriate phrasing and loading mechanism.

The human initially considered putting supporting adapted files in the repository root with agent-prefixed names, such as `agents-security.md` and `claude-security.md`. Separate agent directories were preferred because they expose which agents are supported, avoid root clutter, and make comparisons clearer.

## Skill Architecture Discussion

The human considered whether maintenance skills could be shared across agents. The resulting design is best described as a generic function plus agent-specific adapters:

- Each platform can have a generic-playbook maintenance skill implementing the same agent-neutral workflow.
- Each platform has its own hard-coded adapter skill that writes only that platform's files.
- Agent-specific skills live where that platform's native tooling expects project skills.

For Codex, official repository-local discovery uses:

```text
.agents/skills/<skill-name>/SKILL.md
```

For Claude Code, the human independently confirmed that project-level skills use:

```text
.claude/skills/<skill-name>/SKILL.md
```

The idea of inventing a repository `/codex/skills` location was rejected because following native tooling is preferable to fighting it. A symlink-based bridge was also rejected.

In the Codex guest environment, `.agents` appeared as a read-only, harness-visible path that the human initially could not see from the host. The repository already contained an ignored `_exchange/` directory intended for temporary exchange. The agreed workaround was:

1. Codex creates complete skill directories under `_exchange/` after an approved plan.
2. The human immediately copies those directories into the host-visible `.agents/skills/` location.
3. Codex verifies the copied structure and content.

No extra staging hierarchy under `_exchange/` was needed. Each skill still requires its own directory because the skill format is `<skill-name>/SKILL.md`.

## Codex Skills Created

Two Codex skills were created under `_exchange/` following an approved plan:

- `_exchange/update-generic-playbook/SKILL.md`
- `_exchange/adapt-codex-playbook/SKILL.md`

The human then copied them to:

- `.agents/skills/update-generic-playbook/SKILL.md`
- `.agents/skills/adapt-codex-playbook/SKILL.md`

Verification established that:

- Both destination files existed in the expected repository-local Codex structure.
- Each installed file's SHA-256 hash exactly matched its `_exchange` source.
- Both passed Codex's bundled `quick_validate.py` skill validator.
- Both appeared in Codex's available-skills list for the active session.

When Codex reported `?? .agents/`, the human understandably interpreted the terse report as surprising because there had been no time to stage anything. The clarification was that `?? .agents/` is simply `git status --short` notation for an untracked `.agents/` directory. It was an expected result immediately after the copy, not a problem or an indication that the skills were missing.

## Current Live Files at the Time of This Record

The relevant live generic files were:

- `playbook/agent-playbook.md`
- `playbook/security-policy.md`

The relevant installed Codex skills were:

- `.agents/skills/update-generic-playbook/SKILL.md`
- `.agents/skills/adapt-codex-playbook/SKILL.md`

The agent-specific playbook directories existed but contained no supporting files at the last inspection:

- `agent-playbooks/codex/`
- `agent-playbooks/claude/`

The root `AGENTS.md` and `CLAUDE.md` were empty during the design work. The Codex adapter had not yet been invoked to populate `AGENTS.md`. The Claude skills and Claude adapted playbook had not yet been created.

Immediately before these notes were planned, local Git reported:

- `agent-playbook.md` deleted from the root.
- `.agents/` untracked.
- `playbook/` untracked.

This was consistent with moving the original generic file into `playbook/` and adding repository-local Codex skills. Future agents must inspect current Git state instead of assuming this historical state is unchanged.

## Items Explicitly Deferred or Outside Scope

The human plans to create a separate branching-strategy Markdown file and reference it from the generic playbook. Its contents and implementation were explicitly outside the scope of this discussion.

No automated audit trail beyond ordinary human review was requested. The human considered elaborate adapter explanations unnecessary because a human will review the actual agent-specific files. Adapter plans should therefore explain non-obvious platform choices and exceptions without producing excessive audit ceremony.

The generic-to-agent adapter execution remains explicit. A change to `playbook/` does not authorize or trigger changes to `AGENTS.md`, `CLAUDE.md`, or any agent-specific supporting files.

## Guidance for Future Questions About This Session

When using this record to answer a future question:

1. Treat it as historical rationale, not current authority.
2. Inspect the current `playbook/` files and relevant installed skill before answering questions about present behavior.
3. Distinguish a ratified design decision from a merely discussed alternative.
4. Surface any drift between this record and current files.
5. Do not repair drift unless the human requests a mutation workflow, receives a file-by-file plan, and replies exactly `go`.

