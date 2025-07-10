import { useQuery } from '@tanstack/react-query'
import { getProjects } from '../api/projects'
import type { Project } from '../dto/types'

export const useProjects = () => {
  return useQuery<Project[]>({
    queryKey: ['projects'],
    queryFn: getProjects,
  })
}