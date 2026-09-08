import { createFileRoute } from '@tanstack/react-router'
import { EditWidgetPage } from '@/pages/widgets/EditWidgetPage'
import { RequireRole } from '@/auth/RequireRole'
import { Roles } from '@/auth/roles'

export const Route = createFileRoute('/widgets/$widgetId_/edit')({
  component: () => (
    <RequireRole allowedRoles={[Roles.administrator]}>
      <EditWidgetPage />
    </RequireRole>
  ),
})
