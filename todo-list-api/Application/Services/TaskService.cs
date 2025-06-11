using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

    public async Task<IEnumerable<TaskItem>> GetAllTasksAsync(int? page = 1, int? pageSize = 10, string sort = "")
    {
        if (page is null or < 1) page = 1;
        if (pageSize is null or < 1) pageSize = 10;
        var result = _context.Tasks.AsQueryable();
        if (sort == "asc")
        {
            result = result.OrderBy(t => t.Title);
        }
        else if (sort == "desc")
        {
            result = result.OrderByDescending(t => t.Title);
        }
        result = result.Skip((int)((page - 1) * pageSize)).Take(pageSize.Value);
        return await result.ToListAsync();
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
}