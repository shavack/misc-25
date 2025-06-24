// src/api/tasks.ts
import axios from 'axios'
import type { Task, PaginatedResponse } from '../dto/types'

export const getTasks = async (): Promise<PaginatedResponse<Task>> => {
  const res = await axios.get<PaginatedResponse<Task>>('http://localhost:5000/tasks')
  return res.data
}
export const patchTask = async (task: Partial<Task> & { id: number }) => {
  const response = await axios.patch(`http://localhost:5000/tasks/${task.id}`, task)
  return response.data
}
