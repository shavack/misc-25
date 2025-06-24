import { useDroppable } from '@dnd-kit/core'
import TaskCard from './TaskCard'
import type { Task } from '../dto/types'

interface ColumnProps {
  id: string
  title: string
  tasks: Task[]
}

export default function Column({ id, title, tasks }: ColumnProps) {
  const { setNodeRef } = useDroppable({ id })

  return (
    <div ref={setNodeRef} className="p-6 bg-gray-800 rounded-xl">
      <h2 className="text-xl font-bold mb-4">{title}</h2>
      <div className="flex flex-col gap-2">
        {tasks.map((task) => (
          <TaskCard key={task.id} task={task} />
        ))}
      </div>
    </div>
  )
}
