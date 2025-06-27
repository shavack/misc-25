import Board from '../components/Board'
import AddTaskForm from '../components/AddTaskForm'
import { useTheme } from "../contexts/ThemeContext"
import { themes } from '../themes'

export default function TaskList() {
  const { theme } = useTheme()
  const currentTheme = themes[theme]
  return (
    <div className= {`min-h-screen ${currentTheme.background}`}>
      <header className={`p-4 text-2xl font-bold ${currentTheme.text}`}>Task Manager</header>
      <main className="p-4">
        <AddTaskForm />
        <Board/>
      </main>
    </div>
  )
}