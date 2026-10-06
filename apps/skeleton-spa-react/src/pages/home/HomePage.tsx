import { Link } from '@tanstack/react-router'
import { useAuth } from '@/auth/useAuth'
import { PageHeader } from '@/components/ui/PageHeader'

export function HomePage() {
  const { user } = useAuth()

  return (
    <section className="page-section">
      <PageHeader title="Home" />
      <p>Welcome{user?.displayName ? `, ${user.displayName}` : ''}.</p>
      <p>
        <Link className="btn btn-primary" to="/widgets">
          View Widgets
        </Link>
      </p>
    </section>
  )
}
