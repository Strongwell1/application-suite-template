import { createFileRoute } from '@tanstack/react-router'
import { WidgetDetailPage } from '@/pages/widgets/WidgetDetailPage'

export const Route = createFileRoute('/widgets/$widgetId')({
  component: WidgetDetailPage,
})
