export interface Task {
  id: number
  title: string
  description: string
  isCompleted: boolean,
  createdAt: Date
  updatedAt: Date | null
  completedAt: Date | null
  dueDate: Date | null
}

export interface PaginatedResponse<T> {
  page: number
  pageSize: number
  totalCount: number
  totalPages: number
  items: T[]
}