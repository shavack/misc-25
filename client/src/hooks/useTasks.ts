import { useQuery } from '@tanstack/react-query'
import { getTasks, toggleTask } from '../api/tasks'
import { useMutation, useQueryClient } from '@tanstack/react-query'
import type { PaginatedResponse, Task } from '../dto/types'

export const useTasks = () => {
  return useQuery<PaginatedResponse<Task>>({
    queryKey: ['tasks'],
    queryFn: getTasks,
  })
}

export const useToggleTask = () => {
  const queryClient = useQueryClient()
  return useMutation({
    mutationFn: toggleTask,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['tasks'] })
    },
  })
}