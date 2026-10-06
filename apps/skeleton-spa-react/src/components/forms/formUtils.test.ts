import { describe, expect, it } from 'vitest'
import { ApiError } from '@/api/client'
import { apiErrorMessage, emptyToUndefined, serverFieldError } from './formUtils'

describe('form utilities', () => {
  it('normalizes blank optional text', () => {
    expect(emptyToUndefined('   ')).toBeUndefined()
    expect(emptyToUndefined('  value  ')).toBe('value')
  })

  it('reads field errors from Problem Details', () => {
    const error = new ApiError(400, 'Validation failed.', {
      errors: { name: ['Name is already in use.'] },
    })

    expect(serverFieldError(error, 'name')).toBe('Name is already in use.')
    expect(serverFieldError(error, 'description')).toBeUndefined()
  })

  it('includes a correlation reference in the user-facing message', () => {
    const error = new ApiError(500, 'The request failed.', { correlationId: 'request-123' })

    expect(apiErrorMessage(error)).toBe('The request failed. Reference: request-123')
  })
})
