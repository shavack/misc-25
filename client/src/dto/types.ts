export interface Task {
  id: number
  title: string
  description: string
  isCompleted: boolean
}

export interface PaginatedResponse<T> {
  page: number
  pageSize: number
  totalCount: number
  totalPages: number
  items: T[]
}