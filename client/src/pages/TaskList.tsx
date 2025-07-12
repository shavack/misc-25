import Board from '../components/Board'
import AddTaskForm from '../components/AddTaskForm'
import { useTheme } from "../contexts/ThemeContext"
import { themes } from '../themes'
import { useAuth } from '../contexts/AuthContext'
import LoginForm from '../components/LoginForm'
import { ProjectProvider } from '../contexts/ProjectContext'

export default function TaskList() {
  const { theme } = useTheme()
  const currentTheme = themes[theme]
  const auth = useAuth()

  if (!auth?.token) {
    return <LoginForm />
  }
  return (
    <ProjectProvider>
    <div className= {`min-h-screen ${currentTheme.background}`}>
      <header className={`p-4 text-2xl font-bold ${currentTheme.text}`}>Task Manager</header>
      <main className="p-4">
        <button
          onClick={auth.logout}
          className={`mb-4 p-2 rounded ${currentTheme.button} ${currentTheme.text}`}
        >
          Logout
        </button>
        <h2 className={`text-xl font-semibold mb-4 ${currentTheme.text}`}>Add tasks</h2>
        <AddTaskForm />
        <Board/>
      </main>
    </div>
    </ProjectProvider>
  )
}