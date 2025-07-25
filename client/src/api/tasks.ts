import axios from 'axios'
import type { Task, PaginatedResponse } from '../dto/types'
import { API_BASE_URL } from '../constants/constants'

export const getTasks = async (): Promise<Task[]> => {
  let page = 1
  const pageSize = 10
  let allItems: Task[] = []
  let totalPages = 1
  try {
    do {
      const res = await axios.get<PaginatedResponse<Task>>(
        `${API_BASE_URL}/tasks?page=${page}&pageSize=${pageSize}`
      )
      allItems = allItems.concat(res.data.items)
      totalPages = res.data.totalPages
      page++
    } while (page <= totalPages)
  }
  catch (error) {
    throw new Error("Failed to fetch tasks")
  }
  return allItems
}
export const patchTask = async (task: Partial<Task> & { id: number }) => {
  const response = await axios.patch(`${API_BASE_URL}/tasks/${task.id}`, task)
  return response.data
}

export const createTask = async (task: Partial<Task>) => {
  const response = await axios.post(`${API_BASE_URL}/tasks/`, task)
  return response.data
}

export const deleteTask = async (id: number) => {
  const response = await axios.delete(`${API_BASE_URL}/tasks/${id}`)
  return response.data
}

export const getAllTasksInProjects = async (projectIds: number[]): Promise<Task[]> => {
  let allTasks: Task[] = []
  let page = 1
  let hasMore = true
  const pageSize = 100 // lub mniejszy, zależnie od limitów backendu

  while (hasMore) {
    const query = projectIds.map(id => `projectIds=${id}`).join('&')
    const res = await axios.get<PaginatedResponse<Task>>(
      `${API_BASE_URL}/tasks/tasks-in-projects?page=${page}&pageSize=${pageSize}&${query}`
    )
    allTasks.push(...res.data.items)

    page++
    hasMore = page <= res.data.totalPages
  }

  return allTasks
}
