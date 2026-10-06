export type AppEnvironment = 'development' | 'dev' | 'uat' | 'prod'
export type ApiBaseUrlEnvironment = AppEnvironment | 'unknown'
export type EnvironmentCue = Exclude<ApiBaseUrlEnvironment, 'prod'>

type AppEnv = {
  appEnv: AppEnvironment
  apiBaseUrl: string
  apiBaseUrlEnvironment: ApiBaseUrlEnvironment
  environmentCue: EnvironmentCue | null
  entraClientId: string
  entraAuthority: string
  apiScope: string
  viteMode: string
  isDevBuild: boolean
  isProdBuild: boolean
}

const validAppEnvironments: readonly AppEnvironment[] = ['development', 'dev', 'uat', 'prod']

function parseAppEnvironment(value: string | undefined): AppEnvironment {
  if (validAppEnvironments.includes(value as AppEnvironment)) {
    return value as AppEnvironment
  }

  throw new Error(
    `Invalid VITE_APP_ENV "${value ?? '(missing)'}". Expected one of: ${validAppEnvironments.join(', ')}.`,
  )
}

function readRequiredEnv(
  name: 'VITE_API_BASE_URL' | 'VITE_ENTRA_CLIENT_ID' | 'VITE_ENTRA_AUTHORITY' | 'VITE_API_SCOPE',
): string {
  const value = import.meta.env[name]

  if (!value?.trim()) {
    throw new Error(`Missing ${name}. Check the active .env file for the current Vite mode.`)
  }

  return value.trim()
}

function trimTrailingSlash(value: string): string {
  return value.replace(/\/+$/, '')
}

function parseApiBaseUrlEnvironment(apiBaseUrl: string): ApiBaseUrlEnvironment {
  const hostname = readHostname(apiBaseUrl)

  if (hostname === 'localhost' || hostname === '127.0.0.1' || hostname === '::1') {
    return 'development'
  }

  const hostSegments = hostname.split(/[.-]/)

  if (hostSegments.includes('dev')) return 'dev'
  if (hostSegments.includes('uat')) return 'uat'
  if (hostSegments.includes('prod')) return 'prod'

  return 'unknown'
}

function readHostname(url: string): string {
  try {
    return new URL(url).hostname.toLowerCase()
  } catch {
    return url.toLowerCase()
  }
}

function requireHttpsUrl(value: string, name: string, allowLocalhost = false): string {
  let url: URL

  try {
    url = new URL(value)
  } catch {
    throw new Error(`${name} must be an absolute URL.`)
  }

  const isLocalhost = ['localhost', '127.0.0.1', '::1'].includes(url.hostname.toLowerCase())
  if (url.protocol !== 'https:' && !(allowLocalhost && isLocalhost && url.protocol === 'http:')) {
    throw new Error(
      `${name} must use HTTPS${allowLocalhost ? ' (HTTP is allowed for localhost)' : ''}.`,
    )
  }

  return trimTrailingSlash(url.toString())
}

function getEnvironmentCue(apiEnvironment: ApiBaseUrlEnvironment): EnvironmentCue | null {
  return apiEnvironment === 'prod' ? null : apiEnvironment
}

const apiBaseUrl = requireHttpsUrl(readRequiredEnv('VITE_API_BASE_URL'), 'VITE_API_BASE_URL', true)
const apiBaseUrlEnvironment = parseApiBaseUrlEnvironment(apiBaseUrl)
const appEnv = parseAppEnvironment(import.meta.env.VITE_APP_ENV)

if (apiBaseUrlEnvironment !== 'unknown' && apiBaseUrlEnvironment !== appEnv) {
  throw new Error(
    `VITE_APP_ENV is "${appEnv}" but VITE_API_BASE_URL appears to target "${apiBaseUrlEnvironment}".`,
  )
}

export const env: AppEnv = {
  appEnv,
  apiBaseUrl,
  apiBaseUrlEnvironment,
  environmentCue: getEnvironmentCue(appEnv),
  entraClientId: readRequiredEnv('VITE_ENTRA_CLIENT_ID'),
  entraAuthority: requireHttpsUrl(readRequiredEnv('VITE_ENTRA_AUTHORITY'), 'VITE_ENTRA_AUTHORITY'),
  apiScope: readRequiredEnv('VITE_API_SCOPE'),
  viteMode: import.meta.env.MODE,
  isDevBuild: import.meta.env.DEV,
  isProdBuild: import.meta.env.PROD,
}
