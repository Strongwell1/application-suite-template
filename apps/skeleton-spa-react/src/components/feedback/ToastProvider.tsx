import { type ReactNode, useCallback, useMemo, useRef, useState } from 'react'
import { ToastContext, type ToastKind } from './toastContext'

type ToastMessage = {
  id: number
  kind: ToastKind
  message: string
}

export function ToastProvider({ children }: { children: ReactNode }) {
  const [messages, setMessages] = useState<ToastMessage[]>([])
  const nextId = useRef(0)

  const dismiss = useCallback((id: number) => {
    setMessages((current) => current.filter((m) => m.id !== id))
  }, [])

  const showToast = useCallback(
    (kind: ToastKind, message: string) => {
      const id = ++nextId.current
      setMessages((current) => [...current, { id, kind, message }])
      window.setTimeout(() => dismiss(id), 5000)
    },
    [dismiss],
  )

  const value = useMemo(() => ({ showToast }), [showToast])

  return (
    <ToastContext.Provider value={value}>
      {children}
      <div className="toast-stack" aria-live="polite" aria-relevant="additions">
        {messages.map((m) => (
          <div className={`app-toast ${m.kind}`} key={m.id}>
            <span>{m.message}</span>
            <button type="button" onClick={() => dismiss(m.id)} aria-label="Dismiss message">
              ×
            </button>
          </div>
        ))}
      </div>
    </ToastContext.Provider>
  )
}
