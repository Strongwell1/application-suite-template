import { useMutation, useQueryClient } from '@tanstack/react-query'
import { updateWidget, type UpdateWidgetRequest } from '../api/widgetsApi'
import { queryKeys } from '../api/queryKeys'
import { useAuth } from '../auth/useAuth'

export function useUpdateWidget(id: string) {
  const { getAccessToken } = useAuth()
  const queryClient = useQueryClient()

  return useMutation({
    mutationFn: (request: UpdateWidgetRequest) => updateWidget(id, request, getAccessToken),
    onSuccess: () => {
      void queryClient.invalidateQueries({ queryKey: queryKeys.widgets.detail(id) })
      // Prefix match invalidates every page of the widgets list, regardless of `page`.
      void queryClient.invalidateQueries({ queryKey: queryKeys.widgets.lists() })
    },
  })
}
