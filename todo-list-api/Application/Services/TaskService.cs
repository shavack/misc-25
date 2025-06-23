using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TodoListApi.Application.Dtos;
using TodoListApi.Data;
using TodoListApi.Domain;

namespace TodoListApi.Application.Services;

public class TaskService : ITaskService
{
    private readonly AppDbContext _context;

    public TaskService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedResultDto<TaskItem>> GetAllTasksAsync(TaskQueryParams task)
    {
        var page = task.Page;
        var pageSize = task.PageSize;
        var sortBy = task.SortBy;
        var sortOrder = task.SortOrder;
        var isCompleted = task.IsCompleted;
        var title = task.Title;
        var dueDateFrom = task.DueDateFrom;
        var dueDateTo = task.DueDateTo;

        if (page is null or < 1) page = 1;
        if (pageSize is null or < 1) pageSize = 10;
        var result = _context.Tasks.AsQueryable();
        if (task.SortBy is not null)
        {
            result = sortBy.ToLower() switch
            {
                "duedate" => sortOrder.ToLower() switch
                {
                    "asc" => result.OrderBy(t => t.DueDate),
                    "desc" => result.OrderByDescending(t => t.DueDate),
                    _ => result
                },
                "title" => sortOrder.ToLower() switch
                {
                    "asc" => result.OrderBy(t => t.Title),
                    "desc" => result.OrderByDescending(t => t.Title),
                    _ => result
                },
                _ => sortOrder.ToLower() switch
                {
                    "asc" => result.OrderBy(t => t.Title),
                    "desc" => result.OrderByDescending(t => t.Title),
                    _ => result
                },
            };

        }
        if (isCompleted.HasValue)
        {
            result = result.Where(t => t.IsCompleted == isCompleted.Value);
        }
        if (!string.IsNullOrEmpty(title))
        {
            result = result.Where(t => t.Title.ToLower().Contains(title.ToLower()));
        }
        if (dueDateFrom.HasValue)
        {
            result = result.Where(t => t.DueDate >= dueDateFrom.Value);
        }
        if (dueDateTo.HasValue)
        {
            result = result.Where(t => t.DueDate <= dueDateTo.Value);
        }
        result = result.Skip((int)((page - 1) * pageSize)).Take(pageSize.Value);
        var resultPaginated = new PaginatedResultDto<TaskItem>
        {
            Page = page.Value,
            PageSize = pageSize.Value,
            TotalCount = await _context.Tasks.CountAsync(),
            TotalPages = (int)Math.Ceiling((double)await _context.Tasks.CountAsync() / pageSize.Value),
            Items = await result.ToListAsync(),
        };
        return resultPaginated;
    }

    public async Task<TaskItem> GetTaskByIdAsync(int id)
    {
        return await _context.Tasks.FindAsync(id);
    }

    public async Task<TaskItem> AddTaskAsync(TaskItem taskItem)
    {
        _context.Tasks.Add(taskItem);
        await _context.SaveChangesAsync();
        return taskItem;
    }

    public async Task UpdateTaskAsync(int id, TaskItem taskItem)
    {
        if (id != taskItem.Id)
        {
            throw new ArgumentException("Task ID mismatch");
        }

        _context.Entry(taskItem).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteTaskAsync(int id)
    {
        var taskItem = await _context.Tasks.FindAsync(id);
        if (taskItem == null)
        {
            throw new KeyNotFoundException("Task not found");
        }

        _context.Tasks.Remove(taskItem);
        await _context.SaveChangesAsync();
    }

    public async Task<TaskStatisticsDto> GetStatisticsAsync(DateTime? fromDate = null, DateTime? toDate = null, string title = "", bool? isCompleted = null)
    {
        var allTasks = _context.Tasks.AsQueryable();
        if (fromDate != null)
        {
            allTasks = allTasks.Where(t => t.CreatedAt >= fromDate.Value);
        }
        if (toDate != null)
        {
            allTasks = allTasks.Where(t => t.CreatedAt <= toDate.Value);
        }
        if (isCompleted.HasValue)
        {
            allTasks = allTasks.Where(t => t.IsCompleted == isCompleted.Value);
        }
        if (!string.IsNullOrEmpty(title))
        {
            allTasks = allTasks.Where(t => t.Title.ToLower().Contains(title.ToLower()));
        }
        var totalTasks = await allTasks.CountAsync();
        var completedTasks = await allTasks.CountAsync(t => t.IsCompleted);
        var pendingTasks = totalTasks - completedTasks;

        return new TaskStatisticsDto
        {
            TotalTasks = totalTasks,
            CompletedTasks = completedTasks,
            PendingTasks = pendingTasks,
        };
    }

    public Task SetCompleteAsync(int id)
    {
        var taskItem = _context.Tasks.Find(id);
        if (taskItem == null)
        {
            throw new KeyNotFoundException("Task not found");
        }
        taskItem.IsCompleted = true;
        _context.Tasks.Update(taskItem);
        return _context.SaveChangesAsync();
    }

    public async Task PatchTaskAsync(PatchTaskItemDto taskItem)
    {
        var existingTask = await _context.Tasks.FindAsync(taskItem.Id);
        if (existingTask == null)
        {
            throw new KeyNotFoundException("Task not found");
        }
        existingTask.Title = taskItem.Title ?? existingTask.Title;
        existingTask.Description = taskItem.Description ?? existingTask.Description;
        existingTask.IsCompleted = taskItem.IsCompleted ?? existingTask.IsCompleted;

        _context.Tasks.Update(existingTask);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteTasksAsync(DeleteTasksDto deleteTasksDto)
    {
        var tasksToDelete = await _context.Tasks.Where(t => deleteTasksDto.TaskIds.Contains(t.Id)).ToListAsync();
        if (tasksToDelete.Count != deleteTasksDto.TaskIds.Length)
        {
            throw new KeyNotFoundException("All provided task IDs are not found.");
        }

        _context.Tasks.RemoveRange(tasksToDelete);
        await _context.SaveChangesAsync();
    }

    public async Task<List<TaskItem>> GetOverdueTasksAsync(OverdueTasksDto overdueTasksDto)
    {
        var page = overdueTasksDto.Page ?? 1;
        var pageSize = overdueTasksDto.PageSize ?? 10;

        var overdueTasks = await _context.Tasks
            .Where(t => t.DueDate < DateTime.Now && !t.IsCompleted)
            .Skip((int)((page - 1) * pageSize)).Take(pageSize)
            .ToListAsync();
        return overdueTasks;
    }
    
    public async Task ToggleCompletionAsync(int id)
    {
        var taskItem = await _context.Tasks.FindAsync(id);
        if (taskItem == null)
        {
            throw new KeyNotFoundException("Task not found");
        }
        taskItem.IsCompleted = !taskItem.IsCompleted;
        _context.Tasks.Update(taskItem);
        await _context.SaveChangesAsync();
    }
}