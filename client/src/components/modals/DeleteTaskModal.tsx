import type { Task } from "../../dto/types"
import { useDeleteTask } from "../../hooks/useTasks"

export default function DeleteTaskModal({
  task,
  onClose
}: {
  task: Task
  onClose: () => void
}) {
  const mutation = useDeleteTask()

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault()
    mutation.mutate(
      task.id,
      { onSuccess: onClose }
    )
  }

  return (
    <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
      <div className="bg-white dark:bg-gray-800 p-6 rounded-lg shadow-lg w-[400px]">
        <h2 className="text-xl font-bold mb-4">Edit Task</h2>
        <form onSubmit={handleSubmit} className="flex flex-col gap-4">
          <text className="text-gray-700 dark:text-gray-300 mb-4">
            Are you sure you want to delete the task <b>{task.title}</b>? This action cannot be undone.
          </text>
          <div className="flex justify-end gap-2">
            <button
              type="button"
              onClick={onClose}
              className="bg-gray-300 text-black px-4 py-2 rounded hover:bg-gray-400"
            >
              Cancel
            </button>
            <button
              type="submit"
              className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600"
            >
              Delete
            </button>
          </div>
        </form>
      </div>
    </div>
  )
}
