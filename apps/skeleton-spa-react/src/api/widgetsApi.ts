import { apiFetch, apiFetchBlob } from './client'
import type { GetAccessToken } from './client'

// ─── Domain types ─────────────────────────────────────────────────────────────

export const widgetConstraints = {
  nameMaxLength: 255,
  descriptionMaxLength: 1000,
  attachmentMaxBytes: 10 * 1024 * 1024,
} as const

export type Widget = {
  id: string
  name: string
  description: string | null
  createdUtc: string
  createdById: string
  createdByDisplayName: string
  updatedUtc: string | null
}

export type WidgetAttachment = {
  id: string
  fileName: string
  contentType: string
  fileSizeBytes: number
  uploadedUtc: string
}

export type WidgetDetail = Widget & {
  attachments: WidgetAttachment[]
}

export type PagedWidgets = {
  items: Widget[]
  totalCount: number
  page: number
  pageSize: number
}

export type CreateWidgetRequest = {
  name: string
  description?: string | null
}

export type UpdateWidgetRequest = {
  name: string
  description?: string | null
}

export type CreateWidgetResponse = {
  id: string
}

// ─── API functions ─────────────────────────────────────────────────────────────

export function listWidgets(getAccessToken: GetAccessToken, page = 1, pageSize = 25) {
  return apiFetch<PagedWidgets>(`/widgets?page=${page}&pageSize=${pageSize}`, getAccessToken)
}

export function getWidget(id: string, getAccessToken: GetAccessToken) {
  return apiFetch<WidgetDetail>(`/widgets/${id}`, getAccessToken)
}

export function createWidget(request: CreateWidgetRequest, getAccessToken: GetAccessToken) {
  return apiFetch<CreateWidgetResponse>('/widgets', getAccessToken, {
    method: 'POST',
    body: JSON.stringify(request),
  })
}

export function updateWidget(
  id: string,
  request: UpdateWidgetRequest,
  getAccessToken: GetAccessToken,
) {
  return apiFetch<void>(`/widgets/${id}`, getAccessToken, {
    method: 'PUT',
    body: JSON.stringify(request),
  })
}

export function deleteWidget(id: string, getAccessToken: GetAccessToken) {
  return apiFetch<void>(`/widgets/${id}`, getAccessToken, {
    method: 'DELETE',
  })
}

export function uploadWidgetAttachment(
  widgetId: string,
  file: File,
  getAccessToken: GetAccessToken,
) {
  const formData = new FormData()
  formData.append('file', file)

  return apiFetch<void>(`/widgets/${encodeURIComponent(widgetId)}/attachments`, getAccessToken, {
    method: 'POST',
    body: formData,
  })
}

export function downloadWidgetAttachment(
  widgetId: string,
  attachmentId: string,
  getAccessToken: GetAccessToken,
) {
  return apiFetchBlob(
    `/widgets/${encodeURIComponent(widgetId)}/attachments/${encodeURIComponent(attachmentId)}`,
    getAccessToken,
  )
}
