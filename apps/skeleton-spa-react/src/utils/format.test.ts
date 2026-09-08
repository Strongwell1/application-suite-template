import { describe, it, expect } from 'vitest'
import { formatDateTime } from './format'

describe('formatDateTime', () => {
  it('returns "Not set" for null', () => {
    expect(formatDateTime(null)).toBe('Not set')
  })

  it('formats a valid ISO date string', () => {
    const result = formatDateTime('2024-01-15T00:00:00Z')
    expect(result).toMatch(/Jan/)
    expect(result).toMatch(/2024/)
  })
})
