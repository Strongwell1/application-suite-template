export function LoadingBlock({ label = 'Loading' }: { label?: string }) {
  return (
    <div className="query-state">
      <div className="spinner small" aria-hidden="true" />
      <span>{label}</span>
    </div>
  )
}

export function ErrorBlock({ message }: { message: string }) {
  return (
    <div className="query-state error">
      <strong>Unable to load data.</strong>
      <span>{message}</span>
    </div>
  )
}

export function EmptyState({ title, message }: { title: string; message: string }) {
  return (
    <div className="empty-state">
      <h2>{title}</h2>
      <p>{message}</p>
    </div>
  )
}
