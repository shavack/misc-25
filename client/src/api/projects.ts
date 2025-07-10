import axios from 'axios'
import type { Project, PaginatedResponse } from '../dto/types'
import { API_BASE_URL } from '../constants/constants'

export const getProjects = async (): Promise<Project[]> => {
  let page = 1
  const pageSize = 10
  let allItems: Project[] = []
  let totalPages = 1

  do {
    const res = await axios.get<PaginatedResponse<Project>>(
      `${API_BASE_URL}/projects?page=${page}&pageSize=${pageSize}`
    )
    allItems = allItems.concat(res.data.items)
    totalPages = res.data.totalPages
    page++
  } while (page <= totalPages)

  return allItems
}