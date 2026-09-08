// Date / number formatting utilities

function toUtcSafe(value: string): Date {
  const hasTimezone = /[Zz]$|[+-]\d{2}:\d{2}$/.test(value)
  return new Date(hasTimezone ? value : value + 'Z')
}

export function formatDateTime(value: string | null | undefined): string {
  if (!value) return 'Not set'
  try {
    return new Intl.DateTimeFormat(undefined, {
      dateStyle: 'medium',
      timeStyle: 'short',
    }).format(toUtcSafe(value))
  } catch {
    return value
  }
}
