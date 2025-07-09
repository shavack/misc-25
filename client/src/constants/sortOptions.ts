export const sortOptions = [
  { value: 'titleAsc', label: 'Title (A-Z)' },
  { value: 'titleDesc', label: 'Title (Z-A)' },
  { value: 'createdAt', label: 'Created date' },
  { value: 'dueDate', label: 'Due date' }
] as const

export type SortOption = typeof sortOptions[number]['value']