import {
  InteractionRequiredAuthError,
  type AccountInfo,
  type AuthenticationResult,
} from '@azure/msal-browser'
import { type ReactNode, useCallback, useEffect, useMemo, useState } from 'react'
import { apiTokenRequest, msalClient } from './msalClient'
import { type ApiTokenClaims, decodeJwtPayload, toAuthUser, type AuthUser } from './tokenClaims'
import { hasUserAccess } from './roles'
import { AuthContext, type AuthContextValue, type AuthStatus } from './authContext'

type AuthSession = {
  account: AccountInfo
  user: AuthUser
}

let authStartupPromise: Promise<AuthSession | null> | null = null

function pickAccount(result: AuthenticationResult | null): AccountInfo | null {
  if (result?.account) return result.account
  return msalClient.getActiveAccount() ?? msalClient.getAllAccounts()[0] ?? null
}

function isApiAccessToken(result: AuthenticationResult | null): result is AuthenticationResult {
  return Boolean(
    result?.accessToken &&
    apiTokenRequest.scopes.every((scope) =>
      result.scopes.some((s) => s.toLowerCase() === scope.toLowerCase()),
    ),
  )
}

function authErrorMessage(error: unknown): string {
  if (
    typeof error === 'object' &&
    error !== null &&
    'errorCode' in error &&
    error.errorCode === 'interaction_in_progress'
  ) {
    return 'A Microsoft sign-in interaction is already in progress. Close any extra sign-in windows or tabs, then refresh this page.'
  }

  if (error instanceof InteractionRequiredAuthError) {
    return 'Sign-in succeeded, but the API access token still requires interaction. Check that the SPA app registration has the access_as_user delegated permission and that consent has been granted.'
  }

  return error instanceof Error ? error.message : 'Authentication failed.'
}

async function acquireApiToken(
  account: AccountInfo,
  redirectOnInteraction: boolean,
): Promise<string> {
  try {
    const result = await msalClient.acquireTokenSilent({ ...apiTokenRequest, account })
    return result.accessToken
  } catch (error) {
    if (error instanceof InteractionRequiredAuthError && redirectOnInteraction) {
      await msalClient.acquireTokenRedirect({ ...apiTokenRequest, account })
    }
    throw error
  }
}

async function initializeAuthSession(): Promise<AuthSession | null> {
  await msalClient.initialize()
  const redirectResult = await msalClient.handleRedirectPromise()
  const account = pickAccount(redirectResult)

  if (!account) {
    await msalClient.loginRedirect()
    return null
  }

  msalClient.setActiveAccount(account)

  const accessToken = isApiAccessToken(redirectResult)
    ? redirectResult.accessToken
    : await acquireApiToken(account, false)

  const user = toAuthUser(decodeJwtPayload<ApiTokenClaims>(accessToken))

  return { account, user }
}

function initializeAuthOnce(): Promise<AuthSession | null> {
  authStartupPromise ??= initializeAuthSession()
  return authStartupPromise
}

export function AuthProvider({ children }: { children: ReactNode }) {
  const [status, setStatus] = useState<AuthStatus>('initializing')
  const [session, setSession] = useState<AuthSession | null>(null)
  const [error, setError] = useState<string | null>(null)

  useEffect(() => {
    let isMounted = true

    async function initializeAuth() {
      try {
        const authSession = await initializeAuthOnce()

        if (!isMounted || !authSession) return

        setSession(authSession)
        setStatus(hasUserAccess(authSession.user.roles) ? 'authenticated' : 'unauthorized')
      } catch (authError) {
        if (!isMounted) return
        setError(authErrorMessage(authError))
        setStatus('error')
      }
    }

    void initializeAuth()

    return () => {
      isMounted = false
    }
  }, [])

  const getAccessToken = useCallback(async () => {
    const account = session?.account ?? msalClient.getActiveAccount()

    if (!account) {
      await msalClient.loginRedirect()
      throw new Error('Sign-in is required.')
    }

    return acquireApiToken(account, true)
  }, [session?.account])

  const signIn = useCallback(async () => {
    const account =
      session?.account ?? msalClient.getActiveAccount() ?? msalClient.getAllAccounts()[0]

    if (account) {
      await msalClient.acquireTokenRedirect({ ...apiTokenRequest, account })
      return
    }

    await msalClient.loginRedirect()
  }, [session?.account])

  const signOut = useCallback(async () => {
    await msalClient.logoutRedirect({
      account: session?.account ?? msalClient.getActiveAccount() ?? undefined,
    })
  }, [session?.account])

  const value = useMemo<AuthContextValue>(
    () => ({ status, user: session?.user ?? null, error, getAccessToken, signIn, signOut }),
    [error, getAccessToken, session?.user, signIn, signOut, status],
  )

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>
}
