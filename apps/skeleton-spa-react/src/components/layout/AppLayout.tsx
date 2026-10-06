import { Link, Outlet } from '@tanstack/react-router'
import { useAuth } from '@/auth/useAuth'
import { env } from '@/env'
import { AuthGate } from '@/auth/AuthGate'

// TODO: Replace with your app's branding
const APP_TITLE = 'Skeleton App'
const APP_SUBTITLE = 'Workflow Management'

export function AppLayout() {
  const { signOut, user } = useAuth()

  return (
    <AuthGate>
      <div className="app-shell">
        <header className="app-header">
          <div className="masthead-band">
            <div className="masthead">
              <div className="app-title-block">
                <span className="app-title">{APP_TITLE}</span>
                <span className="app-subtitle">{APP_SUBTITLE}</span>
                {env.environmentCue && (
                  <span className="environment-pill" title={`API: ${env.apiBaseUrl}`}>
                    {env.environmentCue}
                  </span>
                )}
              </div>

              <div className="user-menu">
                <div>
                  <strong>{user?.displayName}</strong>
                  <span>{user?.email}</span>
                </div>
                <button
                  className="btn btn-outline-secondary btn-sm"
                  type="button"
                  onClick={() => void signOut()}
                >
                  Sign out
                </button>
              </div>
            </div>
          </div>

          <div className="nav-band">
            <div className="nav-inner">
              <nav className="main-nav" aria-label="Primary navigation">
                <Link activeProps={{ className: 'active' }} to="/">
                  Home
                </Link>
                <Link activeProps={{ className: 'active' }} to="/widgets">
                  Widgets
                </Link>
              </nav>
            </div>
          </div>
        </header>

        <main className="content-shell">
          <Outlet />
        </main>
      </div>
    </AuthGate>
  )
}
