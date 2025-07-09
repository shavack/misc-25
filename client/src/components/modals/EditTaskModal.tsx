import { useState } from "react"
import type { Task } from "../../dto/types"
import { usePatchTask } from "../../hooks/useTasks"

export default function EditTaskModal({
  task,
  onClose
}: {
  task: Task
  onClose: () => void
}) {
  const [title, setTitle] = useState(task.title)
  const [description, setDescription] = useState(task.description)
  const [dueDate, setDueDate] = useState(task.dueDate)
  const mutation = usePatchTask()

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault()
    mutation.mutate(
      { 
        id: task.id, 
        title, 
        description, 
        dueDate: dueDate ? new Date(dueDate).toISOString().slice(0, 10) : "" 
      },
      { onSuccess: onClose }
    )
  }

  return (
    <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
      <div className="bg-white dark:bg-gray-800 p-6 rounded-lg shadow-lg w-[400px]">
        <h2 className="text-xl font-bold mb-4">Edit Task</h2>
        <form onSubmit={handleSubmit} className="flex flex-col gap-4">
          <input
            className="p-2 rounded border"
            type="text"
            value={title}
            onChange={(e) => setTitle(e.target.value)}
            required
          />
          <input
            className="p-2 rounded border"
            type="text"
            value={description}
            onChange={(e) => setDescription(e.target.value)}
            required
          />
          <input
            className="p-2 rounded border"
            type="date"
            value={
              dueDate instanceof Date
                ? dueDate.toISOString().slice(0, 10)
                : dueDate || ""
            }
            onChange={(e) => setDueDate(e.target.value)}
          />
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
              Save
            </button>
          </div>
        </form>
      </div>
    </div>
  )
}
