import { useMutation, useQueryClient } from '@tanstack/react-query'
import { createWidget, type CreateWidgetRequest } from '../api/widgetsApi'
import { useAuth } from '../auth/useAuth'
import { queryKeys } from '../api/queryKeys'

export function useCreateWidget() {
  const { getAccessToken } = useAuth()
  const queryClient = useQueryClient()

  return useMutation({
    mutationFn: (request: CreateWidgetRequest) => createWidget(request, getAccessToken),
    onSuccess: () => {
      // Prefix match invalidates every page of the widgets list, regardless of `page`.
      void queryClient.invalidateQueries({ queryKey: queryKeys.widgets.lists() })
    },
  })
}
