import { useNavigate } from '@tanstack/react-router'
import { useForm } from '@tanstack/react-form'
import { z } from 'zod'
import { useCreateWidget } from '@/hooks/useCreateWidget'
import { useToast } from '@/components/feedback/useToast'
import { BusyOverlay } from '@/components/feedback/BusyOverlay'
import { PageHeader } from '@/components/ui/PageHeader'
import { TextField, TextAreaField } from '@/components/forms/FormFields'
import {
  apiErrorMessage,
  emptyToUndefined,
  fieldErrorMessage,
  serverFieldError,
} from '@/components/forms/formUtils'
import { widgetConstraints } from '@/api/widgetsApi'

// `description` stays a required string here (never undefined) so its type matches the
// form's string default value — blank is normalized to `null` at submit time instead.
const schema = z.object({
  name: z
    .string()
    .trim()
    .min(1, 'Name is required')
    .max(
      widgetConstraints.nameMaxLength,
      `Name must be ${widgetConstraints.nameMaxLength} characters or fewer`,
    ),
  description: z
    .string()
    .max(
      widgetConstraints.descriptionMaxLength,
      `Description must be ${widgetConstraints.descriptionMaxLength} characters or fewer`,
    ),
})

export function CreateWidgetPage() {
  const navigate = useNavigate()
  const { mutateAsync: createWidget, isPending } = useCreateWidget()
  const { showToast } = useToast()

  const form = useForm({
    defaultValues: { name: '', description: '' },
    onSubmit: async ({ value }) => {
      const parsed = schema.safeParse(value)

      if (!parsed.success) return

      try {
        const result = await createWidget({
          name: parsed.data.name,
          description: emptyToUndefined(parsed.data.description) ?? null,
        })
        showToast('success', 'Widget created successfully.')
        await navigate({ to: '/widgets/$widgetId', params: { widgetId: result.id } })
      } catch (err) {
        for (const field of ['name', 'description'] as const) {
          const message = serverFieldError(err, field)
          if (message) {
            form.setFieldMeta(field, (previous) => ({
              ...previous,
              errorMap: { ...previous.errorMap, onSubmit: message },
            }))
          }
        }

        showToast('error', apiErrorMessage(err))
      }
    },
    validators: {
      onChange: schema,
    },
  })

  return (
    <section className="page-section">
      <PageHeader title="New Widget" />

      <div className="card" style={{ maxWidth: 600 }}>
        <div className="card-body">
          <form
            onSubmit={(e) => {
              e.preventDefault()
              void form.handleSubmit()
            }}
          >
            <form.Field name="name">
              {(field) => (
                <TextField
                  label="Name"
                  required
                  maxLength={widgetConstraints.nameMaxLength}
                  value={field.state.value}
                  onBlur={field.handleBlur}
                  onChange={(e) => field.handleChange(e.target.value)}
                >
                  {field.state.meta.errors.length > 0 && (
                    <span className="inline-error">
                      {fieldErrorMessage(field.state.meta.errors[0])}
                    </span>
                  )}
                </TextField>
              )}
            </form.Field>

            <form.Field name="description">
              {(field) => (
                <TextAreaField
                  label="Description"
                  placeholder="Optional description"
                  maxLength={widgetConstraints.descriptionMaxLength}
                  value={field.state.value}
                  onBlur={field.handleBlur}
                  onChange={(e) => field.handleChange(e.target.value)}
                >
                  {field.state.meta.errors.length > 0 && (
                    <span className="inline-error">
                      {fieldErrorMessage(field.state.meta.errors[0])}
                    </span>
                  )}
                </TextAreaField>
              )}
            </form.Field>

            <div className="d-flex gap-2">
              <button type="submit" className="btn btn-primary" disabled={isPending}>
                Create Widget
              </button>
              <button
                type="button"
                className="btn btn-outline-secondary"
                onClick={() => void navigate({ to: '/widgets' })}
                disabled={isPending}
              >
                Cancel
              </button>
            </div>

            {isPending && <BusyOverlay label="Creating widget..." />}
          </form>
        </div>
      </div>
    </section>
  )
}
