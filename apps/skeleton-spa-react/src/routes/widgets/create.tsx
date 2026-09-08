import { createFileRoute } from '@tanstack/react-router'
import { CreateWidgetPage } from '@/pages/widgets/CreateWidgetPage'
import { RequireRole } from '@/auth/RequireRole'
import { Roles } from '@/auth/roles'

export const Route = createFileRoute('/widgets/create')({
  component: () => (
    <RequireRole allowedRoles={[Roles.administrator]}>
      <CreateWidgetPage />
    </RequireRole>
  ),
})
