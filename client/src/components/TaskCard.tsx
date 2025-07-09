import type { Task } from '../dto/types'
import { useDraggable } from '@dnd-kit/core'
import { useTheme } from '../contexts/ThemeContext'
import { themes } from '../themes'
import { useDeleteTask } from '../hooks/useTasks'

export default function TaskCard({ task, onEdit }: { task: Task, onEdit?: (task: Task) => void }) {
  const { attributes, listeners, setNodeRef } = useDraggable({
    id: task.id,
    data: { status: task.state == 2 ? 'Completed' : 'Pending' },
  })
  const { theme } = useTheme()
  const style = themes[theme]
  const mutation = useDeleteTask()
  const deleteTask = (id: number) => {
    mutation.mutate(
      id,
      { onSuccess: () => {
        console.log(`Task with id ${id} deleted successfully`)
      },
      onError: (error) => {
        console.error(`Error deleting task with id ${id}:`, error)
      } }   
    )
  }

  return (
    <div
      ref={setNodeRef}
      {...listeners}
      {...attributes}
      className={`p-4 rounded-xl border border-white/20 ${style.text} ${
        task.dueDate && new Date(task.dueDate) < new Date() && task.state != 2 ? `${style.overdue}` : `${style.card}`
      }`}
        >
      <div className="flex justify-between">
        <h3 className={`font-bold ${style.text}`}>{task.title}</h3>
        <span
          className={`text-xs px-2 py-1 rounded-full  ${
            (() => {
              switch (task.state) {
                case 0:
                  return `${style.notStartedBackground} ${style.notStartedText}`
                case 1:
                  return `${style.inProgressBackground} ${style.inProgressText}`
                case 2:
                  return `${style.completedBackground} ${style.completedText}`
                default:
                  return `${style.notStartedBackground} ${style.notStartedText}`
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
        {task.completedAt != null ? <p>completed at: {task.completedAt ? task.completedAt.toString() : ''}</p> : null }
      <p className={`${style.text}`}>{task.description}</p>
      <button
        onClick={() => onEdit && onEdit(task)}
        className="text-sm text-blue-400 hover:underline ml-auto flex justify-end mb-2"
        aria-label="Edit">
      ✏️ Edit
      </button>
      <button
        onClick={() => deleteTask(task.id)}
        className="text-sm text-red-500 hover:underline ml-auto flex justify-end mb-2"
        aria-label="Delete"
      >
        ❌ Delete
      </button>
    </div>
  )
}