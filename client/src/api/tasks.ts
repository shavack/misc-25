// src/api/tasks.ts
import axios from 'axios'

export interface Task {
  id: number
  title: string
  description: string
  isCompleted: boolean
}

export const getTasks = async (): Promise<Task[]> => {
  const res = await axios.get<Task[]>('http://localhost:5000/tasks')
  return res.data
}

export const toggleTask = async (task: Task) => {
  await axios.patch(`http://localhost:5000/tasks/${task.id}`, {
    isCompleted: !task.isCompleted,
  })
}