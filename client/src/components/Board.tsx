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
import { useTheme } from "../contexts/ThemeContext"
import { themes } from '../themes'

export default function Board() {
    const { data, isLoading, error } = useTasks()
    const [activeTask, setActiveTask] = useState<Task | null>(null)
    const { theme } = useTheme()
    const currentTheme = themes[theme]

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
    const tasks = data?.items ?? []
    const notStartedTasks = tasks.filter((t) => t.state == 0 || t.state == null)
    const pendingTasks = tasks.filter((t) => t.state == 1)
    const completedTasks = tasks.filter((t) => t.state === 2)

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
        <div className="flex gap-4 w-full px-4">
        <Column id="Backlog" title="Backlog" tasks={notStartedTasks}/>
        <Column id="In progress" title="In progress" tasks={pendingTasks}  />
        <Column id="Completed" title="Completed" tasks={completedTasks}  />
        </div>
        <DragOverlay>
          {activeTask ? <TaskCard task={activeTask} /> : null}
        </DragOverlay>
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