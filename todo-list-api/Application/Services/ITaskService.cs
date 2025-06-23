using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using TodoListApi.Application.Dtos;
using TodoListApi.Domain;
namespace TodoListApi.Application.Services;

public interface ITaskService
{
    Task<PaginatedResultDto<TaskItem>> GetAllTasksAsync(TaskQueryParams taskQueryParams);
    Task<TaskItem> GetTaskByIdAsync(int id);

    Task SetCompleteAsync(int id);
    Task<TaskItem> AddTaskAsync(TaskItem taskItem);

    Task<TaskStatisticsDto> GetStatisticsAsync(DateTime? fromDate = null, DateTime? toDate = null, string title = "", bool? isCompleted = null);

    Task UpdateTaskAsync(int id, TaskItem taskItem);

    Task PatchTaskAsync(PatchTaskItemDto taskItem);

    Task DeleteTaskAsync(int id);

    Task DeleteTasksAsync(DeleteTasksDto deleteTasksDto);

    Task<List<TaskItem>> GetOverdueTasksAsync(OverdueTasksDto overdueTasksDto);

    Task ToggleCompletionAsync(int id);
}