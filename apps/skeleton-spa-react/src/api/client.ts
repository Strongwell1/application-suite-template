import { env } from '../env'

export type GetAccessToken = () => Promise<string>

export type ProblemDetails = {
  title?: string
  detail?: string
  errors?: Record<string, string[]>
  correlationId?: string
}

export class ApiError extends Error {
  readonly status: number
  readonly details: ProblemDetails | null
  readonly correlationId: string | null

  constructor(status: number, message: string, details: ProblemDetails | null = null) {
    super(message)
    this.name = 'ApiError'
    this.status = status
    this.details = details
    this.correlationId = details?.correlationId ?? null
  }
}

async function readProblemDetails(response: Response): Promise<ProblemDetails | null> {
  const contentType = response.headers.get('content-type') ?? ''

  if (!contentType.includes('/json') && !contentType.includes('+json')) return null

  try {
    return (await response.json()) as ProblemDetails
  } catch {
    return null
  }
}

function errorMessage(status: number, details: ProblemDetails | null): string {
  if (details?.detail) return details.detail
  const firstValidationMessage = details?.errors
    ? Object.values(details.errors).flat().find(Boolean)
    : undefined
  if (firstValidationMessage) return firstValidationMessage
  if (details?.title) return details.title
  return `API request failed with status ${status}.`
}

async function sendRequest(
  path: string,
  getAccessToken: GetAccessToken,
  init: RequestInit = {},
): Promise<Response> {
  const accessToken = await getAccessToken()
  const headers = new Headers(init.headers)

  headers.set('Authorization', `Bearer ${accessToken}`)

  if (init.body && !(init.body instanceof FormData) && !headers.has('Content-Type')) {
    headers.set('Content-Type', 'application/json')
  }

  const response = await fetch(`${env.apiBaseUrl}${path}`, { ...init, headers })

  if (!response.ok) {
    const parsedDetails = await readProblemDetails(response)
    const correlationId = response.headers.get('X-Correlation-Id')
    const details = correlationId
      ? { ...(parsedDetails ?? {}), correlationId: parsedDetails?.correlationId ?? correlationId }
      : parsedDetails
    throw new ApiError(response.status, errorMessage(response.status, details), details)
  }

  return response
}

export async function apiFetch<TResponse>(
  path: string,
  getAccessToken: GetAccessToken,
  init: RequestInit = {},
): Promise<TResponse> {
  const response = await sendRequest(path, getAccessToken, init)

  if (response.status === 204) {
    return undefined as TResponse
  }

  return (await response.json()) as TResponse
}

export async function apiFetchBlob(
  path: string,
  getAccessToken: GetAccessToken,
  init: RequestInit = {},
): Promise<Blob> {
  const response = await sendRequest(path, getAccessToken, init)
  return response.blob()
}
