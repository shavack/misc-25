import { DndContext, closestCenter} from '@dnd-kit/core'
import Column from './Column'
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query'
import axios from 'axios'
import { useTasks } from '../hooks/useTasks'
import type { DragEndEvent } from '@dnd-kit/core'

export default function Board() {
    const queryClient = useQueryClient()
    const { data, isLoading, error } = useTasks()

    const toggleStatus = useMutation({
    mutationFn: (id: number) =>
        axios.patch(`http://localhost:5000/tasks/${id}/toggle-complete`),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['tasks'] }),
    })
    const handleDragEnd = (event: DragEndEvent) => {
    const { active, over } = event
    if (!over || active.data.current?.status === over.id) return

    const taskId = active.id
        toggleStatus.mutate(Number(taskId))
    }

    if (isLoading) return <p>Loading...</p>
    if (error) return <p>Error loading tasks</p>
    const tasks = data?.items ?? []
    const pendingTasks = tasks.filter((t) => !t.isCompleted)
    const completedTasks = tasks.filter((t) => t.isCompleted)

    return (
    <DndContext collisionDetection={closestCenter} onDragEnd={handleDragEnd}>
        <div className="flex gap-4">
        <Column id="Pending" title="Pending tasks" tasks={pendingTasks} />
        <Column id="Completed" title="Completed tasks" tasks={completedTasks} />
        </div>
    </DndContext>
    )
}