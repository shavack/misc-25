export interface Task {
  id: number
  title: string
  description: string
  state: number // 0: Pending, 1: In progress, 2: Completed
  createdAt: Date
  updatedAt: Date | null
  completedAt: Date | null
  dueDate: string | Date
}

export interface PaginatedResponse<T> {
  page: number
  pageSize: number
  totalCount: number
  totalPages: number
  items: T[]
}
export type TaskState = 'Pending' | 'In progress' | 'Completed';

export const taskStateMap: Record<TaskState, number> = {
  'Pending': 0,
  'In progress': 1,
  'Completed': 2
}