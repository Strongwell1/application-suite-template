// Helpers for working with TanStack Form + Zod

import type { ZodError } from 'zod'
import { ApiError } from '@/api/client'

/**
 * Converts a Zod error into a flat map of field path → first error message.
 * Useful for displaying field-level validation errors from server responses.
 */
export function flattenZodErrors(error: ZodError): Record<string, string> {
  return Object.fromEntries(error.errors.map((issue) => [issue.path.join('.'), issue.message]))
}

/**
 * Returns undefined if the string is empty/whitespace, otherwise returns the trimmed value.
 * Useful for optional text fields that should be null when empty.
 */
export function emptyToUndefined(value: string | undefined | null): string | undefined {
  const trimmed = value?.trim()
  return trimmed ? trimmed : undefined
}

/**
 * Unwraps an unknown mutation/query error into a display-ready message.
 * Useful for toast/inline error display without callers checking error shape.
 */
export function apiErrorMessage(error: unknown): string {
  if (error instanceof ApiError) {
    return error.correlationId
      ? `${error.message} Reference: ${error.correlationId}`
      : error.message
  }

  if (error instanceof Error) {
    return error.message
  }

  return 'The request failed.'
}

/** Returns the first backend validation message for a form field, when present. */
export function serverFieldError(error: unknown, field: string): string | undefined {
  return error instanceof ApiError ? error.details?.errors?.[field]?.[0] : undefined
}

export function fieldErrorMessage(error: unknown): string {
  if (typeof error === 'string') return error
  if (typeof error === 'object' && error !== null && 'message' in error) {
    return String(error.message)
  }
  return 'Invalid value.'
}
