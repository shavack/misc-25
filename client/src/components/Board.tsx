import { DndContext, closestCenter} from '@dnd-kit/core'
import Column from './Column'
import { useTasks, usePatchTask } from '../hooks/useTasks'
import type { DragEndEvent } from '@dnd-kit/core'
import ThemeSelector from './ThemeSelector'

export default function Board() {
    const { data, isLoading, error } = useTasks()

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
    <DndContext collisionDetection={closestCenter} onDragEnd={handleDragEnd}>
        <ThemeSelector />
        <div className="grid grid-cols-3 gap-4 p-4">
        <Column id="Backlog" title="Backlog" tasks={notStartedTasks} />
        <Column id="In progress" title="In progress" tasks={pendingTasks} />
        <Column id="Completed" title="Completed" tasks={completedTasks} />
        </div>
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