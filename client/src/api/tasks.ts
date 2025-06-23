// src/api/tasks.ts
import axios from 'axios'
import type { Task, PaginatedResponse } from '../dto/types'

export const getTasks = async (): Promise<PaginatedResponse<Task>> => {
  const res = await axios.get<PaginatedResponse<Task>>('http://localhost:5000/tasks')
  return res.data
}

export const toggleTask = async (task: Task) => {
  await axios.patch(`http://localhost:5000/tasks/${task.id}`, {
    isCompleted: !task.isCompleted,
  })
}