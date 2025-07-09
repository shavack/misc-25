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
import {type Task} from '../dto/types'
import EditTaskModal from './modals/EditTaskModal'
import DeleteTaskModal from './modals/DeleteTaskModal'
import { sortOptions, type SortOption } from '../constants/sortOptions'

export default function Board() {
    const { data, isLoading, error } = useTasks()
    const [activeTask, setActiveTask] = useState<Task | null>(null)
    const [taskBeingEdited, setTaskBeingEdited] = useState<Task | null>(null)
    const [taskBeingDeleted, setTaskBeingDeleted] = useState<Task | null>(null)
    const [sortOption, setSortOption] = useState<SortOption>('titleAsc')

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
    const sortedTasks = [...tasks].sort((a, b) => {
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