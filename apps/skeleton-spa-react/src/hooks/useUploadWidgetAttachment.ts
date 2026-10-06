import { useMutation, useQueryClient } from '@tanstack/react-query'
import { uploadWidgetAttachment } from '../api/widgetsApi'
import { queryKeys } from '../api/queryKeys'
import { useAuth } from '../auth/useAuth'

export function useUploadWidgetAttachment(widgetId: string) {
  const { getAccessToken } = useAuth()
  const queryClient = useQueryClient()

  return useMutation({
    mutationFn: (file: File) => uploadWidgetAttachment(widgetId, file, getAccessToken),
    onSuccess: () =>
      queryClient.invalidateQueries({ queryKey: queryKeys.widgets.detail(widgetId) }),
  })
}
