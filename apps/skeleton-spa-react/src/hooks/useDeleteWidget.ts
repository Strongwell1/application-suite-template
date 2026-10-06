import { useMutation, useQueryClient } from '@tanstack/react-query'
import { deleteWidget } from '../api/widgetsApi'
import { queryKeys } from '../api/queryKeys'
import { useAuth } from '../auth/useAuth'

export function useDeleteWidget() {
  const { getAccessToken } = useAuth()
  const queryClient = useQueryClient()

  return useMutation({
    mutationFn: (id: string) => deleteWidget(id, getAccessToken),
    onSuccess: (_data, id) => {
      queryClient.removeQueries({ queryKey: queryKeys.widgets.detail(id) })
      // Prefix match invalidates every page of the widgets list, regardless of `page`.
      void queryClient.invalidateQueries({ queryKey: queryKeys.widgets.lists() })
    },
  })
}
