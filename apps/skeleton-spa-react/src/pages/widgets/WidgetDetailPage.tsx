import { useState } from 'react'
import { Link, useNavigate, useParams } from '@tanstack/react-router'
import { useWidgetDetail } from '@/hooks/useWidgetDetail'
import { useDeleteWidget } from '@/hooks/useDeleteWidget'
import { useToast } from '@/components/feedback/useToast'
import { PageHeader } from '@/components/ui/PageHeader'
import { LoadingBlock, ErrorBlock } from '@/components/feedback/QueryState'
import { BusyOverlay } from '@/components/feedback/BusyOverlay'
import { formatDateTime } from '@/utils/format'
import { apiErrorMessage } from '@/components/forms/formUtils'
import { useAuth } from '@/auth/useAuth'
import { hasAdministratorAccess } from '@/auth/roles'
import { useUploadWidgetAttachment } from '@/hooks/useUploadWidgetAttachment'
import { useDownloadWidgetAttachment } from '@/hooks/useDownloadWidgetAttachment'
import { widgetConstraints, type WidgetAttachment } from '@/api/widgetsApi'
import { formatFileSize } from '@/utils/files'

export function WidgetDetailPage() {
  const { widgetId } = useParams({ from: '/widgets/$widgetId' })
  const { data: widget, isLoading, error } = useWidgetDetail(widgetId)
  const { mutateAsync: deleteWidget, isPending: isDeleting } = useDeleteWidget()
  const { showToast } = useToast()
  const navigate = useNavigate()
  const { user } = useAuth()
  const canAdminister = Boolean(user && hasAdministratorAccess(user.roles))

  async function handleDelete() {
    if (!window.confirm('Delete this widget? This cannot be undone.')) return

    try {
      await deleteWidget(widgetId)
      showToast('success', 'Widget deleted.')
      await navigate({ to: '/widgets' })
    } catch (err) {
      showToast('error', apiErrorMessage(err))
    }
  }

  return (
    <section className="page-section">
      <PageHeader
        title={widget?.name ?? 'Widget Detail'}
        actions={
          widget && canAdminister ? (
            <>
              <Link
                className="btn btn-outline-secondary btn-sm"
                to="/widgets/$widgetId/edit"
                params={{ widgetId: widget.id }}
              >
                Edit
              </Link>
              <button
                type="button"
                className="btn btn-outline-danger btn-sm"
                onClick={() => void handleDelete()}
                disabled={isDeleting}
              >
                Delete
              </button>
            </>
          ) : undefined
        }
      />

      {isLoading && <LoadingBlock label="Loading widget…" />}
      {error && <ErrorBlock message={apiErrorMessage(error)} />}

      {widget && (
        <div className="card">
          <div className="card-body">
            <dl className="row mb-0">
              <dt className="col-sm-3">Name</dt>
              <dd className="col-sm-9">{widget.name}</dd>

              <dt className="col-sm-3">Description</dt>
              <dd className="col-sm-9">{widget.description ?? '—'}</dd>

              <dt className="col-sm-3">Created</dt>
              <dd className="col-sm-9">{formatDateTime(widget.createdUtc)}</dd>

              <dt className="col-sm-3">Created By</dt>
              <dd className="col-sm-9">{widget.createdByDisplayName}</dd>

              <dt className="col-sm-3">Last Updated</dt>
              <dd className="col-sm-9">{formatDateTime(widget.updatedUtc)}</dd>
            </dl>
          </div>
        </div>
      )}

      {widget && <AttachmentSection widgetId={widget.id} attachments={widget.attachments} />}
    </section>
  )
}

function AttachmentSection({
  attachments,
  widgetId,
}: {
  attachments: WidgetAttachment[]
  widgetId: string
}) {
  const [selectedFile, setSelectedFile] = useState<File | null>(null)
  const { mutateAsync: upload, isPending: isUploading } = useUploadWidgetAttachment(widgetId)
  const { mutateAsync: download, isPending: isDownloading } = useDownloadWidgetAttachment(widgetId)
  const { showToast } = useToast()

  async function handleUpload() {
    if (!selectedFile) return
    if (selectedFile.size > widgetConstraints.attachmentMaxBytes) {
      showToast('error', 'The selected file exceeds the 10 MB size limit.')
      return
    }

    try {
      await upload(selectedFile)
      setSelectedFile(null)
      showToast('success', 'Attachment uploaded.')
    } catch (err) {
      showToast('error', apiErrorMessage(err))
    }
  }

  async function handleDownload(attachment: WidgetAttachment) {
    try {
      const blob = await download(attachment.id)
      const url = URL.createObjectURL(blob)
      const anchor = document.createElement('a')
      anchor.href = url
      anchor.download = attachment.fileName
      document.body.append(anchor)
      anchor.click()
      anchor.remove()
      window.setTimeout(() => URL.revokeObjectURL(url), 0)
    } catch (err) {
      showToast('error', apiErrorMessage(err))
    }
  }

  return (
    <div className="card mt-4">
      <div className="card-body">
        <h2 className="h5">Attachments</h2>

        <div className="d-flex flex-wrap align-items-end gap-2 mb-3">
          <label className="form-field flex-grow-1 mb-0">
            <span>Choose a file</span>
            <input
              className="form-control"
              type="file"
              accept=".pdf,.doc,.docx,.xls,.xlsx,.ppt,.pptx,.txt,.md,.csv,.zip,.jpg,.jpeg,.png"
              onChange={(event) => setSelectedFile(event.target.files?.[0] ?? null)}
            />
          </label>
          <button
            className="btn btn-outline-primary"
            type="button"
            disabled={!selectedFile || isUploading}
            onClick={() => void handleUpload()}
          >
            Upload
          </button>
        </div>

        {attachments.length === 0 ? (
          <p className="text-body-secondary mb-0">No attachments.</p>
        ) : (
          <ul className="list-group list-group-flush">
            {attachments.map((attachment) => (
              <li
                className="list-group-item d-flex justify-content-between align-items-center px-0"
                key={attachment.id}
              >
                <span>
                  {attachment.fileName}{' '}
                  <small className="text-body-secondary">
                    ({formatFileSize(attachment.fileSizeBytes)})
                  </small>
                </span>
                <button
                  className="btn btn-sm btn-outline-secondary"
                  type="button"
                  disabled={isDownloading}
                  onClick={() => void handleDownload(attachment)}
                >
                  Download
                </button>
              </li>
            ))}
          </ul>
        )}

        {isUploading && <BusyOverlay label="Uploading attachment..." />}
      </div>
    </div>
  )
}
