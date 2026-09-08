import { useQuery } from '@tanstack/react-query'
import { getWidget } from '../api/widgetsApi'
import { queryKeys } from '../api/queryKeys'
import { useAuth } from '../auth/useAuth'

export function useWidgetDetail(id: string) {
  const { getAccessToken } = useAuth()
  return useQuery({
    queryKey: queryKeys.widgets.detail(id),
    queryFn: () => getWidget(id, getAccessToken),
  })
}
