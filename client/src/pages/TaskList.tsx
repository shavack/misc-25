import Board from '../components/Board'
import AddTaskForm from '../components/AddTaskForm'

export default function TaskList() {
  return (
    <div className="min-h-screen bg-gray-900 text-white">
      <header className="p-4 text-2xl font-bold">Task Manager</header>
      <main className="p-4">
        <AddTaskForm />
        <Board />
      </main>
    </div>
  )
}