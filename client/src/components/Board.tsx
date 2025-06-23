import { DndContext } from '@dnd-kit/core'
import Column from './Column'
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query'
import axios from 'axios'

const fetchTasks = async () => {
  const res = await axios.get('http://localhost:5000/tasks')
  return res.data.items
}

export default function Board() {
  const queryClient = useQueryClient()
  const { data: tasks = [] } = useQuery({ queryKey: ['tasks'], queryFn: fetchTasks })

  const updateTask = useMutation({
    mutationFn: ({ id, isCompleted }: { id: number; isCompleted: boolean }) =>
      axios.patch(`http://localhost:5000/tasks/${id}`, { isCompleted }),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['tasks'] }),
  })

  const handleDragEnd = (event: any) => {
    const { active, over } = event
    if (!over || active.data.current.status === over.id) return

    updateTask.mutate({
      id: active.id,
      isCompleted: over.id === 'Completed',
    })
  }

  const pending = tasks.filter((t: any) => !t.isCompleted)
  const completed = tasks.filter((t: any) => t.isCompleted)

  return (
    <DndContext onDragEnd={handleDragEnd}>
      <div className="flex gap-4 p-4">
        <Column id="Pending" tasks={pending} title ="Pending tasks" />
        <Column id="Completed" tasks={completed} title="Completed tasks" />
      </div>
    </DndContext>
  )
}