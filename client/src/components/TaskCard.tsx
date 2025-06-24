import type { Task } from '../dto/types'
import { useDraggable } from '@dnd-kit/core'

export default function TaskCard({ task }: { task: Task }) {
  const { attributes, listeners, setNodeRef } = useDraggable({
    id: task.id,
    data: { status: task.state == 2 ? 'Completed' : 'Pending' },
  })

  return (
    <div
      ref={setNodeRef}
      {...listeners}
      {...attributes}
      className={`p-4 rounded-xl border border-white/20 ${
        task.dueDate && new Date(task.dueDate) < new Date() && task.state != 2 ? 'bg-red-500' : 'bg-gray-800'
      }`}
        >
      <div className="flex justify-between">
        <h3 className="font-bold text-white">{task.title}</h3>
        <span
          className={`text-xs px-2 py-1 rounded-full ${
            (() => {
              switch (task.state) {
                case 0:
                  return 'bg-gray-300 text-gray-900'
                case 1:
                  return 'bg-yellow-300 text-yellow-900'
                case 2:
                  return 'bg-green-300 text-green-900'
                default:
                  return 'bg-gray-200 text-gray-700'
              }
            })()
          }`}
        >
          {(() => {
            switch (task.state) {
              case 0:
                return 'Not started'
              case 1:
                return 'In progress'
              case 2:
                return 'Completed'
              default:
                return 'undefined'
            }
          })()}
        </span>

      </div>
        <p>created date: {task.dueDate ? task.createdAt.toString() : ''}</p>
        <p>due date: {task.dueDate ? task.dueDate.toString() : ''}</p>
        <p>completed at: {task.completedAt ? task.completedAt.toString() : ''}</p>
      <p className="text-gray-400">{task.description}</p>
    </div>
  )
}