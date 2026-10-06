import { useQuery } from '@tanstack/react-query'
import { listWidgets } from '../api/widgetsApi'
import { queryKeys } from '../api/queryKeys'
import { useAuth } from '../auth/useAuth'

export function useWidgets(page = 1) {
  const { getAccessToken } = useAuth()
  return useQuery({
    queryKey: queryKeys.widgets.list(page),
    queryFn: () => listWidgets(getAccessToken, page),
  })
}
