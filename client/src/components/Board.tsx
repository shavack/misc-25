import { 
  DndContext,
  closestCenter,
  DragOverlay,
  PointerSensor,
  useSensor,
  useSensors } from '@dnd-kit/core'
import Column from './Column'
import { usePatchTask, useTasksInProjects } from '../hooks/useTasks'
import { useProjects } from '../hooks/useProjects'
import type { DragEndEvent, } from '@dnd-kit/core'
import ThemeSelector from './ThemeSelector'
import TaskCard from "./TaskCard"
import { useState } from "react"
import {type Task, type TaskFilters} from '../dto/types'
import EditTaskModal from './modals/EditTaskModal'
import DeleteTaskModal from './modals/DeleteTaskModal'
import { sortOptions, type SortOption } from '../constants/sortOptions'
import { taskStateMap }  from '../dto/types'
import FilterTasksModal from './modals/FilterTasksModal'
import { useProjectContext } from '../contexts/ProjectContext'

export default function Board() {
    const { data: projects, isLoading: loadingProjects } = useProjects()
    const projectIds = projects?.map(p => p.id) ?? []
    const { data: tasks, isLoading: loadingTasks } = useTasksInProjects(projectIds)    
    const [activeTask, setActiveTask] = useState<Task | null>(null)
    const [taskBeingEdited, setTaskBeingEdited] = useState<Task | null>(null)
    const [taskBeingDeleted, setTaskBeingDeleted] = useState<Task | null>(null)
    const [sortOption, setSortOption] = useState<SortOption>('titleAsc')
    const [tagsInput, setTagsInput] = useState("")
    const { currentProjectID, setCurrentProjectID } = useProjectContext()

    const [filters, setFilters] = useState<TaskFilters>({
      title: '',
      description: '',
      status: null,
      dueFrom: '',
      dueTo: '',
      createdFrom: '',
      createdTo: '',
      tags: []
    })
    const sensors = useSensors(
      useSensor(PointerSensor, {
        activationConstraint: { distance: 5 }
      })
    )

    const mutation = usePatchTask()
    const handleDragEnd = (event: DragEndEvent) => {
        const { active, over } = event
        if (!over || active.id === over.id) return

        const newState = mapColumnIdToTaskState(String(over.id))
        mutation.mutate({ id: active.id as number, state: newState })
    }

    if (loadingProjects || loadingTasks) return <p>Loading...</p>
    if (!projects || !tasks) return <p>No data available</p>
    //if (error) return <p>Error loading tasks</p>

    //console.log("Tasks:", tasks)
    const taskInCurrentProject = tasks.filter((task) => currentProjectID === -1 || task.projectId === currentProjectID)

    const filteredTasks = taskInCurrentProject.filter((task) => {
      const matchesTitle = task.title.toLowerCase().includes(filters.title.toLowerCase())
      const matchesDescription = task.description && task.description.toLowerCase().includes(filters.description.toLowerCase())
      const matchesStatus = filters.status === null || task.state === taskStateMap[filters.status]

      const created = new Date(task.createdAt)
      const due = task.dueDate ? new Date(task.dueDate) : null

      const createdValid =
        (!filters.createdFrom || created >= new Date(filters.createdFrom)) &&
        (!filters.createdTo || created <= new Date(filters.createdTo))

      const dueValid =
        (!filters.dueFrom || (due && due >= new Date(filters.dueFrom))) &&
        (!filters.dueTo || (due && due <= new Date(filters.dueTo)))

      const tagsValid =
        filters.tags.length === 0 ||
        filters.tags.some(filterTag =>
          task.tags.some(taskTag =>
              taskTag.trim().toLowerCase().includes(filterTag.trim().toLowerCase())
          )
        )

      return matchesTitle && matchesDescription && matchesStatus && createdValid && dueValid && tagsValid
    })

    const sortedTasks = [...filteredTasks].sort((a, b) => {
      switch (sortOption) {
        case 'titleAsc':
          return a.title.localeCompare(b.title)
        case 'titleDesc':
          return b.title.localeCompare(a.title)
        case 'createdAt':
          return new Date(a.createdAt).getTime() - new Date(b.createdAt).getTime()
        case 'dueDate':
          return (a.dueDate ? new Date(a.dueDate).getTime() : Infinity) -
                (b.dueDate ? new Date(b.dueDate).getTime() : Infinity)
        default:
          return 0
      }
    })
    const notStartedTasks = sortedTasks.filter((t) => t.state == 0 || t.state == null)
    const pendingTasks = sortedTasks.filter((t) => t.state == 1)
    const completedTasks = sortedTasks.filter((t) => t.state === 2)


    return (
    <DndContext
      sensors = {sensors} 
      collisionDetection={closestCenter} 
      onDragStart={(event) => {
        const task = tasks.find((t) => t.id == event.active.id)
        setActiveTask(task || null)
    }} 
      onDragEnd={handleDragEnd}
      onDragCancel = {() => setActiveTask(null)}>
        <h2 className={`text-xl font-semibold mb-4`}>Change theme</h2>
        <ThemeSelector />
        <h2 className={`text-xl font-semibold mb-4`}>Sort tasks</h2>
        <div className="mb-4">
            <label className="mr-2 text-sm font-medium">Sort by:</label>
            <select
                value={sortOption}
                onChange={(e) => setSortOption(e.target.value as SortOption)}
                className="p-2 rounded border"
            >
                {sortOptions.map((opt) => (
                    <option key={opt.value} value={opt.value}>{opt.label}</option>
                ))}
            </select>
        </div>
        <h2 className={`text-xl font-semibold mb-4`}>Filter tasks</h2>
        <FilterTasksModal
          filters={filters}
          setFilters={setFilters}
          tagsInput={tagsInput}
          setTagsInput={setTagsInput}
        />
        <h2 className={`text-xl font-semibold mb-4`}>Choose project</h2>
        <select
          value={currentProjectID}
          onChange={(e) => setCurrentProjectID(Number(e.target.value))}
          className="p-2 border rounded mb-4"
        >
          <option value={-1}>All Projects</option>
          {projects.map((project) => (
            <option key={project.id} value={project.id}>
              {project.name}
            </option>
          ))}
        </select>
        <h2 className={`text-xl font-semibold mb-4`}>Your taskboard</h2>
        <div className="flex gap-4 w-full px-4">
        <Column id="Backlog" title={`Backlog ${notStartedTasks.length}/${(tasks?.length ?? 0)}`} tasks={notStartedTasks} onEdit={(task) => setTaskBeingEdited(task)} onDelete = {(task) => setTaskBeingDeleted(task)}/>
        <Column id="In progress" title={`In progress ${pendingTasks.length}/${(tasks?.length ?? 0)}`} tasks={pendingTasks} onEdit={(task) => setTaskBeingEdited(task)} onDelete = {(task) => setTaskBeingDeleted(task)}/>
        <Column id="Completed" title={`Completed ${completedTasks.length}/${(tasks?.length ?? 0)}`} tasks={completedTasks} onEdit={(task) => setTaskBeingEdited(task)} onDelete = {(task) => setTaskBeingDeleted(task)}/>
        </div>
        <DragOverlay>
          {activeTask ? <TaskCard task={activeTask} /> : null}
        </DragOverlay>
        {taskBeingEdited && (
        <EditTaskModal
          task={taskBeingEdited}
          onClose={() => setTaskBeingEdited(null)}
        />
        )}
        {taskBeingDeleted && (
        <DeleteTaskModal
          task={taskBeingDeleted}
          onClose={() => setTaskBeingDeleted(null)}
        />
        )}
    </DndContext>
    )
}

const mapColumnIdToTaskState = (id: string): number => {
  switch (id) {
    case 'Pending':
      return 0
    case 'In progress':
      return 1
    case 'Completed':
      return 2 
    default:
      return 0
  }
}