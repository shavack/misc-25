import { 
  DndContext,
  closestCenter,
  DragOverlay,
  PointerSensor,
  useSensor,
  useSensors } from '@dnd-kit/core'
import Column from './Column'
import { useTasks, usePatchTask } from '../hooks/useTasks'
import type { DragEndEvent, } from '@dnd-kit/core'
import ThemeSelector from './ThemeSelector'
import TaskCard from "./TaskCard"
import { useState } from "react"
import {type Task, type TaskState} from '../dto/types'
import EditTaskModal from './modals/EditTaskModal'
import DeleteTaskModal from './modals/DeleteTaskModal'
import { sortOptions, type SortOption } from '../constants/sortOptions'
import { taskStateMap }  from '../dto/types'

export default function Board() {
    const { data, isLoading, error } = useTasks()
    const [activeTask, setActiveTask] = useState<Task | null>(null)
    const [taskBeingEdited, setTaskBeingEdited] = useState<Task | null>(null)
    const [taskBeingDeleted, setTaskBeingDeleted] = useState<Task | null>(null)
    const [sortOption, setSortOption] = useState<SortOption>('titleAsc')
    const [titleFilter, setTitleFilter] = useState("")
    const [descriptionFilter, setDescriptionFilter] = useState("")
    const [statusFilter, setStatusFilter] = useState<TaskState | null>(null)
    const [createdFrom, setCreatedFrom] = useState("")
    const [createdTo, setCreatedTo] = useState("")
    const [dueFrom, setDueFrom] = useState("")
    const [dueTo, setDueTo] = useState("")

    const sensors = useSensors(
      useSensor(PointerSensor, {
        activationConstraint: { distance: 5 }
      })
    )

    const mutation = usePatchTask()
    const handleDragEnd = (event: DragEndEvent) => {
        const { active, over } = event
        if (!over || active.id === over.id) return

        const newState = mapColumnIdToTaskState(String(over.id)) // np. 'backlog' => 0
        mutation.mutate({ id: active.id as number, state: newState })
    }

    if (isLoading) return <p>Loading...</p>
    if (error) return <p>Error loading tasks</p>
    const tasks = data ?? []

    const filteredTasks = tasks.filter((task) => {
      const matchesTitle = task.title.toLowerCase().includes(titleFilter.toLowerCase())
      const matchesDescription = task.description && task.description.toLowerCase().includes(descriptionFilter.toLowerCase())
      const matchesStatus = statusFilter === null || task.state === taskStateMap[statusFilter]

      const created = new Date(task.createdAt)
      const due = task.dueDate ? new Date(task.dueDate) : null

      const createdValid =
        (!createdFrom || created >= new Date(createdFrom)) &&
        (!createdTo || created <= new Date(createdTo))

      const dueValid =
        (!dueFrom || (due && due >= new Date(dueFrom))) &&
        (!dueTo || (due && due <= new Date(dueTo)))

      return matchesTitle && matchesDescription && matchesStatus && createdValid && dueValid
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
        <ThemeSelector />
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
        <div className="grid grid-cols-4 gap-4 mb-6">

          <div className="flex flex-col">
            <label className="text-sm font-medium mb-1">Filter by title</label>
            <input
              type="text"
              value={titleFilter}
              onChange={(e) => setTitleFilter(e.target.value)}
              className="p-2 rounded border"
            />
          </div>

          <div className="flex flex-col">
            <label className="text-sm font-medium mb-1">Filter by description</label>
            <input
              type="text"
              value={descriptionFilter}
              onChange={(e) => setDescriptionFilter(e.target.value)}
              className="p-2 rounded border"
            />
          </div>

          <div className="flex flex-col">
            <label className="text-sm font-medium mb-1">Status</label>
            <select
              value={statusFilter ?? ""}
              onChange={(e) => setStatusFilter(e.target.value === "" ? null : e.target.value as TaskState)}
              className="p-2 rounded border"
            >
              <option value="">All statuses</option>
              <option value="Pending">Not Started</option>
              <option value="In progress">In Progress</option>
              <option value="Completed">Completed</option>
            </select>
          </div>

          <div className="flex flex-col">
            <label className="text-sm font-medium mb-1">Created from</label>
            <input
              type="date"
              value={createdFrom}
              onChange={(e) => setCreatedFrom(e.target.value)}
              className="p-2 rounded border"
            />
          </div>

          <div className="flex flex-col">
            <label className="text-sm font-medium mb-1">Created to</label>
            <input
              type="date"
              value={createdTo}
              onChange={(e) => setCreatedTo(e.target.value)}
              className="p-2 rounded border"
            />
          </div>

          <div className="flex flex-col">
            <label className="text-sm font-medium mb-1">Due date from</label>
            <input
              type="date"
              value={dueFrom}
              onChange={(e) => setDueFrom(e.target.value)}
              className="p-2 rounded border"
            />
        </div>

        <div className="flex flex-col">
          <label className="text-sm font-medium mb-1">Due date to</label>
          <input
            type="date"
            value={dueTo}
            onChange={(e) => setDueTo(e.target.value)}
            className="p-2 rounded border"
          />
        </div>
        </div>

        <div className="flex gap-4 w-full px-4">
        <Column id="Backlog" title={`Backlog ${notStartedTasks.length}/${tasks.length}`} tasks={notStartedTasks} onEdit={(task) => setTaskBeingEdited(task)} onDelete = {(task) => setTaskBeingDeleted(task)}/>
        <Column id="In progress" title={`In progress ${pendingTasks.length}/${tasks.length}`} tasks={pendingTasks} onEdit={(task) => setTaskBeingEdited(task)} onDelete = {(task) => setTaskBeingDeleted(task)}/>
        <Column id="Completed" title={`Completed ${completedTasks.length}/${tasks.length}`} tasks={completedTasks} onEdit={(task) => setTaskBeingEdited(task)} onDelete = {(task) => setTaskBeingDeleted(task)}/>
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
    case 'Backlog':
      return 0 // Not started
    case 'In progress':
      return 1 // In progress
    case 'Completed':
      return 2 // Completed
    default:
      return 0 // Unknown state
  }
}