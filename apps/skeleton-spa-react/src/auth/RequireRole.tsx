import type { ReactNode } from 'react'
import { useAuth } from './useAuth'
import { hasAnyRole, type AppRole } from './roles'

export function RequireRole({
  allowedRoles,
  children,
}: {
  allowedRoles: readonly AppRole[]
  children: ReactNode
}) {
  const { user } = useAuth()

  if (!user || !hasAnyRole(user.roles, allowedRoles)) {
    return (
      <section className="page-section">
        <div className="empty-state">
          <h1>Access Denied</h1>
          <p>Your account does not have access to this page.</p>
        </div>
      </section>
    )
  }

  return children
}
