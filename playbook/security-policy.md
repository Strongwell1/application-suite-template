# Security Policy

## Purpose

This policy defines mandatory workspace, filesystem, credential, and network boundaries for agents. Its prohibitions cannot be overridden by `go` or by another human instruction. If requested work crosses a prohibited boundary, the agent explains the blocker and waits for the human to change the repository or configuration so work can continue safely.

## Workspace Boundary

The agreed top-level workspace folder is the project boundary. It should match the Git repository root. Agents may use read-only inspection to establish both locations before planning work.

If the starting folder is not a Git root, Git reports a different root, or the agent discovers a nested repository or worktree, surface the condition as a likely layout mistake and obtain human confirmation of the workspace root before proceeding.

Anything outside the agreed workspace boundary is off-limits for project inspection and mutation. Do not follow a symbolic link whose resolved target is outside the boundary.

Within the workspace, read-only access is allowed unless a path or its likely content may enter a prohibited area. Repository mutations remain governed by the planning and `go` requirements in the primary playbook.

## Runtime Exceptions

The agent's own home directory, such as `/home/agent`, may be used for normal agent runtime state and tool caches. The guest VM's `/tmp` directory may be used for transient tool files. Expected incidental writes to those locations from an approved action do not need individual approval.

These exceptions do not permit access to another user's home directory, human-owned credentials, host-mounted content, or unrelated temporary data.

## Filesystem and Private Resources

Never attempt to discover, inspect, mount, probe, or access:

- The host filesystem outside the guest VM.
- Private LAN or non-public WAN resources.
- Mapped, shared, or network drives outside the authorized workspace.
- Resources hidden behind filesystem or network permissions.

Technical visibility never implies permission. Do not attempt to bypass a permission boundary.

Before running a build, test, or other tool that may contact a prohibited resource, inspect its configuration when safely possible. If its behavior remains uncertain, surface the issue and do not run it. Human approval cannot authorize prohibited access.

## Credentials and Sensitive Content

Agents may use only the credentials and resources required for their own model backend and agent services. Never search for, inspect, copy, or use human credentials or credential stores, even if permissions accidentally make them accessible.

If a workspace path or content may contain prohibited credentials or sensitive material, stop before investigating further, notify the human, and wait for direction. Local hardcoded test values are not automatically prohibited, but uncertainty must be surfaced.

If a credential or prohibited value appears accidentally in command output, do not reproduce, quote, echo, or log it again. Report only where it was encountered and wait for direction.

## External Access

Interactions required for the agent's own model backend and agent-service resources are allowed.

All other external interactions are read-only. Agents may read or download publicly available resources, including public GitHub and Azure content. Agents may not send data to, trigger, publish to, upload to, or otherwise mutate an external service. Disable optional telemetry when possible; do not use a tool if it cannot operate without a prohibited external write.

Any private or authenticated access to GitHub or Azure is strictly prohibited. Never access private repositories, tenants, APIs, or services, and never attempt authentication using human credentials. Remote actions required from GitHub, Azure, or another private service must remain human actions.
