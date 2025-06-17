using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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

    public async Task<PaginatedResultDto<TaskItem>> GetAllTasksAsync(int? page = 1, int? pageSize = 10, string sort = "", bool? isCompleted = false, string title = "")
    {
        if (page is null or < 1) page = 1;
        if (pageSize is null or < 1) pageSize = 10;
        var result = _context.Tasks.AsQueryable();

        if (!string.IsNullOrEmpty(sort))
        {
            result = sort.ToLower() switch
            {
                "asc" => result.OrderBy(t => t.Title),
                "desc" => result.OrderByDescending(t => t.Title),
                _ => result
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
    
    public async Task<TaskStatisticsDto> GetStatisticsAsync()
    {
        var totalTasks = await _context.Tasks.CountAsync();
        var completedTasks = await _context.Tasks.CountAsync(t => t.IsCompleted);
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
}