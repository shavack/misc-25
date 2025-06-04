using System.Collections.Generic;
using System.Threading.Tasks;
using TodoListApi.Models;

public interface ITaskService
{
    Task<IEnumerable<TaskItem>> GetAllTasksAsync();
    Task<TaskItem> GetTaskByIdAsync(int id);
    Task<TaskItem> AddTaskAsync(TaskItem taskItem);
    Task UpdateTaskAsync(int id, TaskItem taskItem);
    Task DeleteTaskAsync(int id);
}