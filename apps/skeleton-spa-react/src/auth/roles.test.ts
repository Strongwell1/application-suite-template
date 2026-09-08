import { describe, expect, it } from 'vitest'
import { hasAdministratorAccess, hasRole, hasUserAccess, Roles } from './roles'

describe('role access', () => {
  it.each([Roles.developer, Roles.administrator, Roles.user])('grants basic access to %s', (role) =>
    expect(hasUserAccess([role])).toBe(true),
  )

  it('grants administrator access to administrators and developers', () => {
    expect(hasAdministratorAccess([Roles.administrator])).toBe(true)
    expect(hasAdministratorAccess([Roles.developer])).toBe(true)
    expect(hasAdministratorAccess([Roles.user])).toBe(false)
  })

  it('does not treat an administrator as a developer', () => {
    expect(hasRole([Roles.administrator], Roles.developer)).toBe(false)
  })
})
