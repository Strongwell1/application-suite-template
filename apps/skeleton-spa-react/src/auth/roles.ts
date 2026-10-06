// TODO: Replace these role strings with the actual app role values from your
// Entra ID app registration manifest.

export const Roles = {
  developer: 'Global.Developer',
  administrator: 'Skeleton.Administrator',
  user: 'Skeleton.User',
} as const

export type AppRole = (typeof Roles)[keyof typeof Roles]

export function hasRole(roles: readonly string[], role: AppRole): boolean {
  return roles.includes(role)
}

export function hasAnyRole(roles: readonly string[], allowedRoles: readonly AppRole[]): boolean {
  return allowedRoles.some((role) => hasRole(roles, role))
}

export function hasUserAccess(roles: readonly string[]): boolean {
  return hasAnyRole(roles, [Roles.developer, Roles.administrator, Roles.user])
}

export function hasAdministratorAccess(roles: readonly string[]): boolean {
  return roles.includes(Roles.developer) || roles.includes(Roles.administrator)
}
