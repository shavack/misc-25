import type { Task } from '../dto/types'
import { useDraggable } from '@dnd-kit/core'

export default function TaskCard({ task }: { task: Task }) {
  const { attributes, listeners, setNodeRef } = useDraggable({
    id: task.id,
    data: { status: task.isCompleted ? 'Completed' : 'Pending' },
  })

  return (
    <div
      ref={setNodeRef}
      {...listeners}
      {...attributes}
      className="bg-gray-800 p-4 rounded-xl border border-white/20"
    >
      <div className="flex justify-between">
        <h3 className="font-bold text-white">{task.title}</h3>
        <span
          className={`text-xs px-2 py-1 rounded-full ${
            task.isCompleted ? 'bg-green-300 text-green-900' : 'bg-yellow-300 text-yellow-900'
          }`}
        >
          {task.isCompleted ? 'Completed' : 'Pending'}
        </span>
      </div>
      <p className="text-gray-400">{task.description}</p>
    </div>
  )
}