// Add a key entry for each query in the app.
// Convention: top-level key = resource name, sub-keys = filter/id variants.

export const queryKeys = {
  widgets: {
    all: ['widgets'] as const,
    lists: () => ['widgets', 'list'] as const,
    list: (page: number) => ['widgets', 'list', { page }] as const,
    details: () => ['widgets', 'detail'] as const,
    detail: (id: string) => ['widgets', 'detail', id] as const,
  },
}
