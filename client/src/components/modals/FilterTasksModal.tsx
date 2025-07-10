import type { TaskFilters } from '../../dto/types'

type Props = {
  filters: TaskFilters
  setFilters: React.Dispatch<React.SetStateAction<TaskFilters>>
  tagsInput: string
  setTagsInput: React.Dispatch<React.SetStateAction<string>>
}

export default function FilterTasksModal({ filters, setFilters, tagsInput, setTagsInput }: Props) {
  return (
    <div className="grid grid-cols-4 gap-4 mb-6">
        <div className="flex flex-col">
            <label className="text-sm font-medium mb-1">Filter by title</label>
            <input
            type="text"
            value={filters.title}
            onChange={(e) => setFilters((prev) => ({ ...prev, title: e.target.value }))}
            className="p-2 rounded border"
            placeholder="Enter task title"
            />
        </div>

        <div className="flex flex-col">
            <label className="text-sm font-medium mb-1">Filter by description</label>
            <input
            type="text"
            value={filters.description}
            onChange={(e) => setFilters((prev) => ({ ...prev, description: e.target.value }))}
            className="p-2 rounded border"
            placeholder="Enter task description"
            />
        </div>

        <div className="flex flex-col">
            <label className="text-sm font-medium mb-1">Status</label>
            <select
            value={filters.status ?? ""}
            onChange={(e) =>
                setFilters((prev) => ({
                ...prev,
                status:
                    e.target.value === ""
                    ? null
                    : (e.target.value as TaskFilters["status"])
                }))
            }
            className="p-2 rounded border"
            >
            <option value="">All statuses</option>
            <option value="Not started">Not Started</option>
            <option value="In progress">In Progress</option>
            <option value="Completed">Completed</option>
            </select>
        </div>

        <div className="flex flex-col">
            <label className="text-sm font-medium mb-1">Created from</label>
            <input
            type="date"
            value={filters.createdFrom}
            onChange={(e) => setFilters((prev) => ({ ...prev, createdFrom: e.target.value }))}
            className="p-2 rounded border"
            />
        </div>

        <div className="flex flex-col">
            <label className="text-sm font-medium mb-1">Created to</label>
            <input
            type="date"
            value={filters.createdTo}
            onChange={(e) => setFilters((prev) => ({ ...prev, createdTo: e.target.value }))}
            className="p-2 rounded border"
            />
        </div>

        <div className="flex flex-col">
            <label className="text-sm font-medium mb-1">Due date from</label>
            <input
            type="date"
            value={filters.dueFrom}
            onChange={(e) => setFilters((prev) => ({ ...prev, dueFrom: e.target.value }))}
            className="p-2 rounded border"
            />
        </div>

        <div className="flex flex-col">
            <label className="text-sm font-medium mb-1">Due date to</label>
            <input
            type="date"
            value={filters.dueTo}
            onChange={(e) => setFilters((prev) => ({ ...prev, dueTo: e.target.value }))}
            className="p-2 rounded border"
            />
        </div>

        <div className="flex flex-col">
            <label className='text-sm font-medium mb-1'>Tags</label>
            <input
                type="text"
                value={tagsInput}
                onChange={(e) => {
                    const inputValue = e.target.value
                    setTagsInput(inputValue)

                    const parsedTags = inputValue
                    .split(',')
                    .map(tag => tag.trim())
                    .filter(tag => tag !== "")

                    setFilters(prev => ({
                    ...prev,
                    tags: parsedTags
                    }))
                }}
                className="p-2 rounded border"
                placeholder="Enter tags separated by commas"
                />
        </div>
    </div>
  )
}
