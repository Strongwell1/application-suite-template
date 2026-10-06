import { createContext } from 'react'

export type ToastKind = 'success' | 'error' | 'warning' | 'info'

export type ToastContextValue = {
  showToast: (kind: ToastKind, message: string) => void
}

export const ToastContext = createContext<ToastContextValue | null>(null)
