import { useQuery } from '@tanstack/react-query'
import { getTasks, patchTask, createTask, deleteTask } from '../api/tasks'
import { useMutation, useQueryClient } from '@tanstack/react-query'
import type { Task } from '../dto/types'

export const useTasks = () => {
  return useQuery<Task[]>({
    queryKey: ['tasks'],
    queryFn: getTasks,
  })
}

export const usePatchTask = () => {
  const queryClient = useQueryClient()

  return useMutation({
    mutationFn: patchTask,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['tasks'] })
    },
  })
}

export const useCreateTask = () =>  {
  const queryClient = useQueryClient()
  return useMutation({
      mutationFn: createTask,
      onSuccess: () => {
        queryClient.invalidateQueries({ queryKey: ['tasks'] })
      }
    })
  }

export const useDeleteTask = () => {
  const queryClient = useQueryClient()
  return useMutation({
    mutationFn: (id: number) => deleteTask(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['tasks'] })
    }
  })
} 