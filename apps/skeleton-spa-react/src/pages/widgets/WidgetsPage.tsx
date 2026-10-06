import { Link, useNavigate } from '@tanstack/react-router'
import { useWidgets } from '@/hooks/useWidgets'
import { PageHeader } from '@/components/ui/PageHeader'
import { LoadingBlock, ErrorBlock, EmptyState } from '@/components/feedback/QueryState'
import { formatDateTime } from '@/utils/format'
import { Route as WidgetsIndexRoute } from '@/routes/widgets/index'
import { useAuth } from '@/auth/useAuth'
import { hasAdministratorAccess } from '@/auth/roles'
import { apiErrorMessage } from '@/components/forms/formUtils'

export function WidgetsPage() {
  const { page = 1 } = WidgetsIndexRoute.useSearch()
  const navigate = useNavigate()
  const { data: widgets, isLoading, error } = useWidgets(page)
  const { user } = useAuth()
  const totalPages = widgets ? Math.max(1, Math.ceil(widgets.totalCount / widgets.pageSize)) : 1

  return (
    <section className="page-section">
      <PageHeader
        title="Widgets"
        actions={
          user && hasAdministratorAccess(user.roles) ? (
            <Link className="btn btn-primary btn-sm" to="/widgets/create">
              New Widget
            </Link>
          ) : undefined
        }
      />

      {isLoading && <LoadingBlock label="Loading widgets…" />}
      {error && <ErrorBlock message={apiErrorMessage(error)} />}

      {widgets && widgets.items.length === 0 && (
        <EmptyState title="No widgets yet" message="Create your first widget to get started." />
      )}

      {widgets && widgets.items.length > 0 && (
        <>
          <table className="table table-hover">
            <thead>
              <tr>
                <th>Name</th>
                <th>Description</th>
                <th>Created</th>
                <th>Created By</th>
              </tr>
            </thead>
            <tbody>
              {widgets.items.map((widget) => (
                <tr key={widget.id}>
                  <td>
                    <Link to="/widgets/$widgetId" params={{ widgetId: widget.id }}>
                      {widget.name}
                    </Link>
                  </td>
                  <td>{widget.description ?? '—'}</td>
                  <td>{formatDateTime(widget.createdUtc)}</td>
                  <td>{widget.createdByDisplayName}</td>
                </tr>
              ))}
            </tbody>
          </table>

          {totalPages > 1 && (
            <div className="pagination-bar d-flex align-items-center gap-2">
              <button
                type="button"
                className="btn btn-sm btn-outline-secondary"
                disabled={page <= 1}
                onClick={() => void navigate({ to: '/widgets', search: { page: page - 1 } })}
              >
                Previous
              </button>
              <span className="pagination-info">
                Page {page} of {totalPages}
              </span>
              <button
                type="button"
                className="btn btn-sm btn-outline-secondary"
                disabled={page >= totalPages}
                onClick={() => void navigate({ to: '/widgets', search: { page: page + 1 } })}
              >
                Next
              </button>
            </div>
          )}
        </>
      )}
    </section>
  )
}
