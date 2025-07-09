// src/api/tasks.ts
import axios from 'axios'
import type { Task, PaginatedResponse } from '../dto/types'

export const getTasks = async (): Promise<Task[]> => {
  let page = 1
  const pageSize = 10
  let allItems: Task[] = []
  let totalPages = 1

  do {
    const res = await axios.get<PaginatedResponse<Task>>(
      `http://localhost:5000/tasks?page=${page}&pageSize=${pageSize}`
    )
    allItems = allItems.concat(res.data.items)
    totalPages = res.data.totalPages
    page++
  } while (page <= totalPages)

  return allItems
}
export const patchTask = async (task: Partial<Task> & { id: number }) => {
  const response = await axios.patch(`http://localhost:5000/tasks/${task.id}`, task)
  return response.data
}

export const createTask = async (task: Partial<Task>) => {
  const response = await axios.post(`http://localhost:5000/tasks/`, task)
  return response.data
}

export const deleteTask = async (id: number) => {
  const response = await axios.delete(`http://localhost:5000/tasks/${id}`)
  return response.data
}