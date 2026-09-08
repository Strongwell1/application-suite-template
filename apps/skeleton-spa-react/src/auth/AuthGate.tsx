import type { ReactNode } from 'react'
import { useAuth } from './useAuth'

export function AuthGate({ children }: { children: ReactNode }) {
  const { error, signIn, status } = useAuth()

  if (status === 'initializing') {
    return (
      <main className="auth-shell">
        <div className="loading-panel">
          <div className="spinner" aria-hidden="true" />
          <h1>Signing in</h1>
          <p>Checking your access.</p>
        </div>
      </main>
    )
  }

  if (status === 'unauthorized') {
    return (
      <main className="auth-shell">
        <div className="loading-panel">
          <h1>Access Denied</h1>
          <p>Your account does not have access to this application.</p>
          <button className="btn btn-primary" type="button" onClick={() => void signIn()}>
            Try again
          </button>
        </div>
      </main>
    )
  }

  if (status === 'error') {
    return (
      <main className="auth-shell">
        <div className="loading-panel">
          <h1>Sign-in failed</h1>
          <p>{error ?? 'Authentication failed.'}</p>
          <button className="btn btn-primary" type="button" onClick={() => void signIn()}>
            Sign in
          </button>
        </div>
      </main>
    )
  }

  return children
}
