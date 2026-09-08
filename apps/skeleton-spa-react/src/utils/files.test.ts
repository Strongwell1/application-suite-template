import { describe, expect, it } from 'vitest'
import { formatFileSize } from './files'

describe('formatFileSize', () => {
  it.each([
    [0, '0 B'],
    [1023, '1023 B'],
    [1024, '1.0 KB'],
    [1024 * 1024, '1.0 MB'],
  ])('formats %s bytes', (bytes, expected) => {
    expect(formatFileSize(bytes)).toBe(expected)
  })

  it('handles invalid sizes', () => {
    expect(formatFileSize(-1)).toBe('Unknown size')
  })
})
