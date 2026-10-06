import { useMutation } from '@tanstack/react-query'
import { downloadWidgetAttachment } from '../api/widgetsApi'
import { useAuth } from '../auth/useAuth'

export function useDownloadWidgetAttachment(widgetId: string) {
  const { getAccessToken } = useAuth()

  return useMutation({
    mutationFn: (attachmentId: string) =>
      downloadWidgetAttachment(widgetId, attachmentId, getAccessToken),
  })
}
