import { useDroppable } from '@dnd-kit/core'
import TaskCard from './TaskCard'
import type { Task } from '../dto/types'
import { themes } from '../themes'
import { useTheme } from '../contexts/ThemeContext'

interface ColumnProps {
  id: string
  title: string
  tasks: Task[],
}

export default function Column({ id, title, tasks }: ColumnProps) {
  const { setNodeRef } = useDroppable({ id })
  const { theme } = useTheme()
  const style = themes[theme]
  
  return (
    <div ref={setNodeRef} className={`flex-1 p-6 rounded-xl ${style.background} ${style.text}`}>
      <h2 className="text-xl font-bold mb-4">{title}</h2>
      <div className="flex flex-col gap-2">
        {tasks.map((task) => (
          <TaskCard key={task.id} task={task} />
        ))}
      </div>
    </div>
  )
}
