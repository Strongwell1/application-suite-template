export function BusyOverlay({ label }: { label: string }) {
  return (
    <div className="busy-overlay" role="status" aria-live="polite">
      <span className="spinner small" aria-hidden="true" />
      <span>{label}</span>
    </div>
  )
}
