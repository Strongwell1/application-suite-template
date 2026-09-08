# Azure Custom RBAC Roles — Least Privilege Assessment

**Status: Proposed — dev-tenant testing only, not yet ratified by the company.** All three roles have been tested and confirmed functional in a development environment on a separate non-production tenant. This assessment supports internal review and, if needed, external technical consultation — it does not itself constitute company sign-off.

## Purpose

Neutral technical assessment of three custom Azure RBAC role definitions, each created to enforce least-privilege access for a specific operational task: evaluates whether each role is appropriately scoped, identifies permissions broader than strictly necessary, and notes where further tightening was attempted but found non-functional.

## Background

The three roles address:
- Deploying an Azure App Service application via GitHub Actions, replacing a broader built-in role and eliminating SCM/FTP basic-auth publishing credentials.
- Deploying an Azure Static Web App via GitHub Actions, replacing a manual build-and-deploy workflow that previously required either Contributor-level access or error-prone manual execution.
- Allowing an App Service managed identity to send email via Azure Communication Services (ACS), replacing a stored ACS connection-string key in application environment variables.

## Role 1: App Service Deploy Only

**Workflow context:** GitHub Actions authenticates via service principal and deploys to an existing App Service. SCM and FTP basic-auth publishing credentials are disabled, requiring all deployments to authenticate via service principal. The workflow does not create, modify, or delete the App Service resource itself.

| Permission | Purpose / Notes |
|---|---|
| `Microsoft.Web/sites/read` | Required to read App Service resource properties during deployment. |
| `Microsoft.Web/sites/publish/action` | Core deployment permission — pushes application artifacts. |
| `Microsoft.Web/sites/publishxml/action` | Retrieves the publish profile — required to obtain endpoint/auth details with basic auth disabled. |
| `Microsoft.Web/sites/config/list/action` | Required by the deployment process; confirmed non-functional without it. See Concern below. |

**Scope:** Assignable at subscription level; role assignments made at the individual App Service resource level.

**Strengths:** Tightly scoped to deployment actions only — no ability to create, modify config of, or delete the resource; no slot-management permissions; disabling SCM/FTP basic auth is a meaningful security improvement; standardizing on GitHub Actions removes inconsistent local-deployment risk.

**Concern:** `config/list/action` can return the App Service's full configuration, including application settings and connection strings — broader than the other three permissions in this role. Current environment variables contain no secrets (verified) — only non-sensitive config (environment name, port, CORS origins, public endpoint URLs, non-secret Entra app identifiers). Risk is low today but not evaluated against future environment-variable changes; if secrets are ever added there, this permission would expose them.

**Worth reviewing with a security consultant:** Whether a narrower alternative to `config/list/action` exists; whether a policy/process control should prevent secrets from ever being added to App Service environment variables, given this permission persists regardless.

## Role 2: Static Web App Deploy Only

**Workflow context:** Deploying an Azure Static Web App previously required a manual workflow — selecting environment-specific npm build parameters, then executing the Azure Static Web Apps CLI deploy command with its full parameter set. High-risk due to potential parameter errors. The built-in-role alternative would have required Contributor access, judged unacceptably broad for a deployment-only task.

| Permission | Purpose / Notes |
|---|---|
| `Microsoft.Web/staticSites/Read` | Required to read the Static Web App resource during deployment. |
| `Microsoft.Web/staticSites/publish/action` | Core deployment permission for publishing content. |
| `Microsoft.Web/staticSites/zipdeploy/action` | Required by the deployment mechanism; confirmed non-functional without it. |
| `Microsoft.Web/staticSites/listsecrets/action` | Retrieves the deployment token used to authenticate the deployment; confirmed non-functional without it. See Concern below. |

**Scope:** Assignable at subscription level; role assignments should be made at the individual Static Web App resource level.

**Strengths:** Eliminates a high-risk manual workflow; achieves the deployment goal without Contributor access; no resource-management permissions included (cannot create, update config of, or delete the resource).

**Concern:** `listsecrets/action` retrieves the deployment token itself — a sensitive credential that can be used by anyone who possesses it to deploy arbitrary content to the Static Web App. This is an inherent characteristic of how Static Web App token-based deployment authentication works, not a flaw specific to this role. If the service principal's credentials were compromised, an attacker could retrieve and reuse this token.

**Worth reviewing with a security consultant:** Whether Azure Static Web Apps now support a managed-identity or federated-identity deployment path that would eliminate the need to retrieve/use the deployment token; whether the GitHub Actions service principal's credentials are stored/rotated per organizational requirements.

## Role 3: ACS Email Sender

**Workflow context:** An App Service API sends transactional email via Azure Communication Services. Previously authenticated via a connection-string key stored in App Service environment variables — a risk, since the key could be exposed through configuration exports, diagnostic logs, or inadvertent disclosure. This role lets the App Service's system-assigned managed identity authenticate to ACS directly, extending the same managed-identity pattern already used for SQL Server access. No secrets are stored in environment variables; the ACS endpoint URL remains in configuration but is not itself a secret.

| Permission | Purpose / Notes |
|---|---|
| `Microsoft.Communication/CommunicationServices/Read` | Allows the managed identity to read ACS resource properties, required for the SDK to resolve the endpoint and validate the resource. |
| `Microsoft.Communication/CommunicationServices/Write` | Required for email sending via managed-identity authentication; confirmed non-functional without it. See Concern below. |

**Scope:** `AssignableScopes` targets the specific ACS resource, not the subscription or resource group — the most tightly scoped of the three roles.

**Strengths:** Eliminates a stored secret; consistent with the managed-identity pattern already used for SQL Server; no delete/management permissions; scoped to a specific ACS resource.

**Concern:** `Write` is a control-plane permission — in Azure RBAC terms, `Write` on a resource type typically grants the ability to modify the resource's configuration, not just perform data-plane operations. ACS RBAC granularity currently has no purpose-built data-plane action for sending email via managed identity without also granting `Write` (verified by testing — removing `Write` caused the operation to fail). The managed identity holding this role could theoretically modify the ACS resource configuration, not just send email; the resource-specific scope limits blast radius, but the permission is broader than the intended use case.

**Worth reviewing with a security consultant:** Whether Microsoft has introduced more granular ACS data-plane RBAC roles since this role was designed; whether the `Write` permission is an acceptable risk given the managed identity is system-assigned to a specific App Service and cannot be used outside that service's execution context.

## Summary

| Role | Scope | Least-Privilege Assessment | Primary Open Question |
|---|---|---|---|
| App Service Deploy Only | Per App Service resource | Appropriate — one permission (`config/list`) is broader than the deployment action alone but functionally required and low-risk given current environment-variable configuration. | Whether `config/list` can be replaced with a narrower alternative; whether a policy control on environment-variable secrets should be added. |
| Static Web App Deploy Only | Per Static Web App resource | Appropriate given platform constraints — `listsecrets` is an inherent requirement of token-based deployment. | Whether managed-identity or federated-credential deployment is now supported for Static Web Apps. |
| ACS Email Sender | Per ACS resource (specific) | Appropriate given ACS RBAC granularity limits — `Write` is required with no narrower functional alternative found. | Whether Microsoft has introduced a purpose-built ACS email data-plane role. |

All three roles represent a meaningful improvement over the alternatives evaluated: built-in roles with excessive permissions, stored secrets in environment variables, and complex manual workflows. Permissions flagged as broader than strictly necessary were each tested against narrower alternatives that were confirmed non-functional, or reflect current limitations in Azure's RBAC granularity for the relevant service.
