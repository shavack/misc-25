using System.Collections.Generic;
using System.Threading.Tasks;
using TodoListApi.Application.Dtos;
using TodoListApi.Domain;
namespace TodoListApi.Application.Services;

public interface ITaskService
{
    Task<PaginatedResultDto<TaskItem>> GetAllTasksAsync(int? page = 1, int? pageSize = 1, string sort = "", bool? isCompleted = false, string title = "");
    Task<TaskItem> GetTaskByIdAsync(int id);
    Task<TaskItem> AddTaskAsync(TaskItem taskItem);

    Task<TaskStatisticsDto> GetStatisticsAsync();

    Task UpdateTaskAsync(int id, TaskItem taskItem);
    Task DeleteTaskAsync(int id);
}