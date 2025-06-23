import { useQuery } from '@tanstack/react-query'
import { getTasks, toggleTask } from '../api/tasks'
import { useMutation, useQueryClient } from '@tanstack/react-query'

export const useTasks = () => {
  return useQuery({
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