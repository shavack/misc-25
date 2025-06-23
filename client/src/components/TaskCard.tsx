import type { Task } from '../api/tasks'
import { useToggleTask } from '../hooks/usetasks'

export default function TaskCard({ task }: { task: Task }) {
  const { mutate: toggle } = useToggleTask()

  return (
    <div
      className="p-4 bg-zinc-800 rounded-xl shadow-md mb-2 cursor-pointer hover:bg-zinc-700 transition"
      onClick={() => toggle(task)}
    >
      <div className="flex justify-between items-center">
        <h3 className="font-semibold">{task.title}</h3>
        <span
          className={`text-xs font-bold px-3 py-1 rounded-full ${
            task.isCompleted ? 'bg-green-300 text-green-800' : 'bg-yellow-300 text-yellow-800'
          }`}
        >
          {task.isCompleted ? 'Completed' : 'Pending'}
        </span>
      </div>
      <p className="text-zinc-400 text-sm">{task.description}</p>
    </div>
  )
}