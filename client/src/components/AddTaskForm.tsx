import { useState } from "react"
import { useCreateTask } from "../hooks/useTasks"
import { useTheme } from '../contexts/ThemeContext'
import { themes } from '../themes'

export default function AddTaskForm()
{
    const [title, setTitle] = useState("")
    const [description, setDescription] = useState("")
    const [dueDate, setDueDate] = useState("")
    const mutation = useCreateTask()
    const { theme, setTheme } = useTheme()
    const currentTheme = themes[theme];

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault()
        mutation.mutate(
            { title, description, dueDate },
            {
                onSuccess: () => {
                setTitle("")
                setDueDate("")
                setDescription("")
                }
            }
        )
    }
    return (
    <form onSubmit={handleSubmit} className="mb-4 flex gap-2">
    <div className={`flex flex-col ${currentTheme.background} ${currentTheme.text}`}>
        <label htmlFor="title" className="text-sm font-medium mb-1">
        Title
        </label>
        <input
        id="title"
        type="text"
        value={title}
        onChange={(e) => setTitle(e.target.value)}
        className={`p-2 rounded border ${currentTheme.background}`}
        required
        />
    </div>
    <div className={`flex flex-col ${currentTheme.background} ${currentTheme.text}`}>
        <label htmlFor="description" className="text-sm font-medium mb-1">
        Description
        </label>
        <input
        id="description"
        type="text"
        value={description}
        onChange={(e) => setDescription(e.target.value)}
        className={`p-2 rounded border ${currentTheme.background}`}
        required
        />
    </div>
    <div className={`flex flex-col ${currentTheme.background} ${currentTheme.text}`}>
        <label htmlFor="dueDate" className="text-sm font-medium mb-1">
        Due date
        </label>
        <input
        id="dueDate"
        type="date"
        value={dueDate}
        onChange={(e) => setDueDate(e.target.value)}
        className={`p-2 rounded border ${currentTheme.background}`}
        />
    </div>
    <div className={`flex flex-col ${currentTheme.background} ${currentTheme.text}`}>
        <button
            type="submit"
            className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600 w-full h-full"
        >
            Add Task
        </button>
    </div>  
    </form>
  )
}