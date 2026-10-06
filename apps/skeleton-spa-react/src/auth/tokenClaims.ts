export type ApiTokenClaims = {
  name?: string
  oid?: string
  preferred_username?: string
  roles?: string[] | string
}

export type AuthUser = {
  id: string
  displayName: string
  email: string
  roles: string[]
}

function decodeBase64Url(value: string): string {
  const base64 = value.replace(/-/g, '+').replace(/_/g, '/')
  const padded = base64.padEnd(base64.length + ((4 - (base64.length % 4)) % 4), '=')
  const binary = window.atob(padded)
  const bytes = Uint8Array.from(binary, (character) => character.charCodeAt(0))

  return new TextDecoder().decode(bytes)
}

export function decodeJwtPayload<TClaims>(token: string): TClaims {
  const parts = token.split('.')

  if (parts.length < 2 || !parts[1]) {
    throw new Error('The access token was not a valid JWT.')
  }

  return JSON.parse(decodeBase64Url(parts[1])) as TClaims
}

export function toAuthUser(claims: ApiTokenClaims): AuthUser {
  const roles = Array.isArray(claims.roles)
    ? claims.roles
    : typeof claims.roles === 'string'
      ? [claims.roles]
      : []

  return {
    id: claims.oid ?? '',
    displayName: claims.name ?? claims.preferred_username ?? 'Signed in user',
    email: claims.preferred_username ?? '',
    roles,
  }
}
