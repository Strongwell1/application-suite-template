import { Link } from '@tanstack/react-router'
import { PageHeader } from '@/components/ui/PageHeader'

export function NotFoundPage() {
  return (
    <section className="page-section">
      <PageHeader title="Page not found" />
      <p>The page you're looking for doesn't exist or may have moved.</p>
      <Link className="btn btn-primary" to="/">
        Back to home
      </Link>
    </section>
  )
}
