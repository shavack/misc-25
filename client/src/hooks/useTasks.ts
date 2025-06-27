import { useQuery } from '@tanstack/react-query'
import { getTasks, patchTask, createTask } from '../api/tasks'
import { useMutation, useQueryClient } from '@tanstack/react-query'
import type { PaginatedResponse, Task } from '../dto/types'

export const useTasks = () => {
  return useQuery<PaginatedResponse<Task>>({
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