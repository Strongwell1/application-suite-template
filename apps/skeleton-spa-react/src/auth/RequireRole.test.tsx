import { render, screen } from '@testing-library/react'
import { describe, expect, it, vi } from 'vitest'
import { AuthContext, type AuthContextValue } from './authContext'
import { RequireRole } from './RequireRole'
import { Roles } from './roles'

function renderWithRoles(roles: string[]) {
  const value: AuthContextValue = {
    status: 'authenticated',
    user: { id: 'user-id', displayName: 'Test User', email: 'user@example.com', roles },
    error: null,
    getAccessToken: vi.fn(async () => 'token'),
    signIn: vi.fn(async () => undefined),
    signOut: vi.fn(async () => undefined),
  }

  render(
    <AuthContext.Provider value={value}>
      <RequireRole allowedRoles={[Roles.administrator]}>
        <p>Protected content</p>
      </RequireRole>
    </AuthContext.Provider>,
  )
}

describe('RequireRole', () => {
  it('renders protected content for an allowed role', () => {
    renderWithRoles([Roles.administrator])
    expect(screen.getByText('Protected content')).toBeInTheDocument()
  })

  it('renders access denied for a disallowed role', () => {
    renderWithRoles([Roles.user])
    expect(screen.getByRole('heading', { name: 'Access Denied' })).toBeInTheDocument()
    expect(screen.queryByText('Protected content')).not.toBeInTheDocument()
  })
})
