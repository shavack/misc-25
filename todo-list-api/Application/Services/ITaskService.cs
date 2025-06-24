using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using TodoListApi.Application.Dtos;
using TodoListApi.Domain;
using TodoListApi.Types;
namespace TodoListApi.Application.Services;

public interface ITaskService
{
    Task<PaginatedResultDto<TaskItem>> GetAllTasksAsync(TaskQueryParams taskQueryParams);
    Task<TaskItem> GetTaskByIdAsync(int id);

    Task SetCompleteAsync(int id);
    Task<TaskItem> AddTaskAsync(TaskItem taskItem);

    Task<TaskStatisticsDto> GetStatisticsAsync(DateOnly? fromDate = null, DateOnly? toDate = null, string title = "", TaskState? state = null);

    Task UpdateTaskAsync(int id, TaskItem taskItem);

    Task PatchTaskAsync(PatchTaskItemDto taskItem);

    Task DeleteTaskAsync(int id);

    Task DeleteTasksAsync(DeleteTasksDto deleteTasksDto);

    Task<List<TaskItem>> GetOverdueTasksAsync(OverdueTasksDto overdueTasksDto);

    Task ToggleCompletionAsync(int id);
}