import { createFileRoute } from '@tanstack/react-router'
import { WidgetsPage } from '@/pages/widgets/WidgetsPage'

type WidgetsSearch = {
  page?: number
}

export const Route = createFileRoute('/widgets/')({
  validateSearch: (search: Record<string, unknown>): WidgetsSearch => ({
    page: typeof search.page === 'number' && search.page > 0 ? search.page : undefined,
  }),
  component: WidgetsPage,
})
