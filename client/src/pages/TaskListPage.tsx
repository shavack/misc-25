import { useQuery } from '@tanstack/react-query';
import { api } from '../services/api';

interface Task {
  id: number;
  title: string;
  description: string;
  isCompleted: boolean;
  dueDate?: string;
}

export const TaskListPage = () => {
  const { data: tasks, isLoading } = useQuery({
    queryKey: ['tasks'],
    queryFn: async () => {
      const response = await api.get<{ items: Task[] }>('/tasks');
      return response.data.items;
    },
  });

  if (isLoading) return <p className="p-4">Loading...</p>;

  return (
    <div className="p-6">
      <h1 className="text-2xl font-bold mb-4">Task List</h1>
      <ul className="space-y-2">
        {tasks?.map((task) => (
          <li
            key={task.id}
            className="border p-4 rounded shadow-sm flex justify-between items-center"
          >
            <div>
              <p className="font-semibold">{task.title}</p>
              <p className="text-sm text-gray-600">{task.description}</p>
            </div>
            <span
              className={`text-xs font-medium px-2 py-1 rounded-full ${
                task.isCompleted ? 'bg-green-200 text-green-800' : 'bg-yellow-200 text-yellow-800'
              }`}
            >
              {task.isCompleted ? 'Completed' : 'Pending'}
            </span>
          </li>
        ))}
      </ul>
    </div>
  );
};