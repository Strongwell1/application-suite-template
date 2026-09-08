import { createContext } from 'react'
import type { AuthUser } from './tokenClaims'

export type AuthStatus = 'initializing' | 'authenticated' | 'unauthorized' | 'error'

export type AuthContextValue = {
  status: AuthStatus
  user: AuthUser | null
  error: string | null
  getAccessToken: () => Promise<string>
  signIn: () => Promise<void>
  signOut: () => Promise<void>
}

export const AuthContext = createContext<AuthContextValue | null>(null)
