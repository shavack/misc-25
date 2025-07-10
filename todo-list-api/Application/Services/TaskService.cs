using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using TodoListApi.Application.Dtos;
using TodoListApi.Data;
using TodoListApi.Domain;
using TodoListApi.Types;

namespace TodoListApi.Application.Services;

public class TaskService : ITaskService
{
    private readonly AppDbContext _context;

    private readonly IProjectService _projectService;

    private readonly string[] _tags = new[]
    {
        "work", "personal", "urgent", "low-priority", "high-priority", "home", "school", "health", "finance", "travel", "talarek", "miscellaneous"
    };

    private readonly string[] titles = new[]
    {
        "Complete project report", "Buy groceries", "Schedule doctor appointment", "Finish reading book", "Prepare for meeting",
        "Clean the house", "Plan vacation", "Organize files", "Attend workshop", "Update resume", "Learn new programming language",
        "Write blog post", "Exercise regularly", "Cook dinner", "Call family member"
    };

    private readonly string[] descriptions = new[]
    {
        "This task involves completing the project report by the end of the week.",
        "Remember to buy groceries for the week, including fruits, vegetables, and snacks.",
        "Schedule a doctor appointment for a routine check-up or any health concerns.",
        "Finish reading the book that has been on your list for a while.",
        "Prepare for the upcoming meeting by reviewing notes and gathering necessary documents.",
        "Clean the house, focusing on areas that need attention like dusting and vacuuming.",
        "Plan a vacation by researching destinations, booking flights, and accommodations.",
        "Organize files on your computer or in physical form to improve efficiency.",
        "Attend a workshop to learn new skills or enhance existing ones.",
        "Update your resume with recent experiences and skills acquired.",
        "Learn a new programming language to expand your skill set.",
        "Write a blog post about a topic of interest or expertise.",
        "Exercise regularly to maintain physical health and well-being.",
        "Cook dinner using a new recipe or favorite dish.",
        "Call a family member to catch up and stay connected."
    };

    public TaskService(AppDbContext context, IProjectService projectService)
    {
        _context = context;
        _projectService = projectService;
    }

    public async Task<PaginatedResultDto<TaskItem>> GetAllTasksAsync(TaskQueryParams task)
    {
        var page = task.Page;
        var pageSize = task.PageSize;
        var sortBy = task.SortBy;
        var sortOrder = task.SortOrder;
        var state = task.State;
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
        if (state is not null)
        {
            result = result.Where(t => t.State == state);
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
        if (taskItem.ProjectId == null || await _projectService.GetProjectByIdAsync(taskItem.ProjectId.Value) == null)
            throw new KeyNotFoundException("Project not found");
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

        if (taskItem.ProjectId != null && await _projectService.GetProjectByIdAsync(taskItem.ProjectId.Value) == null)
            throw new KeyNotFoundException("Project not found");

        _context.Entry(taskItem).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteTaskAsync(int id)
    {
        var taskItem = await _context.Tasks.FindAsync(id) ?? throw new KeyNotFoundException("Task not found");
        _context.Tasks.Remove(taskItem);
        await _context.SaveChangesAsync();
    }

    public async Task<TaskStatisticsDto> GetStatisticsAsync(DateOnly? fromDate = null, DateOnly? toDate = null, string title = "", TaskState? taskState = TaskState.NotStarted)
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
        if (taskState is not null)
        {
            allTasks = allTasks.Where(t => t.State == taskState);
        }
        if (!string.IsNullOrEmpty(title))
        {
            allTasks = allTasks.Where(t => t.Title.ToLower().Contains(title.ToLower()));
        }
        var totalTasks = await allTasks.CountAsync();
        var completedTasks = await allTasks.CountAsync(t => t.State == TaskState.Completed);
        var inProgressTasks = await allTasks.CountAsync(t => t.State == TaskState.InProgress);
        var notStartedTasks = totalTasks - completedTasks - inProgressTasks;
        return new TaskStatisticsDto
        {
            TotalTasks = totalTasks,
            CompletedTasks = completedTasks,
            InProgressTasks = inProgressTasks,
            NotStartedTasks = notStartedTasks
        };
    }

    public Task SetCompleteAsync(int id)
    {
        var taskItem = _context.Tasks.Find(id);
        if (taskItem == null)
        {
            throw new KeyNotFoundException("Task not found");
        }
        taskItem.State = TaskState.Completed;
        taskItem.CompletedAt = DateOnly.FromDateTime(DateTime.Now);
        taskItem.LastModifiedAt = DateOnly.FromDateTime(DateTime.Now);
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
        existingTask.State = taskItem.State ?? existingTask.State;
        existingTask.LastModifiedAt = DateOnly.FromDateTime(DateTime.Now);
        existingTask.CompletedAt = taskItem.State == TaskState.Completed ? DateOnly.FromDateTime(DateTime.Now) : null;
        existingTask.DueDate = taskItem.DueDate ?? existingTask.DueDate;
        existingTask.Tags = taskItem.Tags.Length > 0 ? taskItem.Tags : existingTask.Tags;
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
            .Where(t => t.DueDate < DateOnly.FromDateTime(DateTime.Now) && t.State != TaskState.Completed)
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
        taskItem.State = taskItem.State switch
        {
            TaskState.Completed => TaskState.NotStarted,
            TaskState.NotStarted => TaskState.Completed,
            _ => taskItem.State
        };

        if (taskItem.State == TaskState.Completed)
        {
            taskItem.CompletedAt = DateOnly.FromDateTime(DateTime.Now);
        }
        else
        {
            taskItem.CompletedAt = null;
        }
        _context.Tasks.Update(taskItem);
        await _context.SaveChangesAsync();
    }
    
    public async Task<List<TaskItem>> PopulateDatabaseAsync(BulkAddTasksDto bulkAddTasksDto)
    {
        var tasks = new List<TaskItem>();
        var projects = await _projectService.GetAllProjectsAsync(new ProjectQueryParams()) ?? throw new KeyNotFoundException("No projects found. Please create a project first.");

        for (var i = 0; i < bulkAddTasksDto.NumberOfTasks; i++)
        {
            var task = new TaskItem
            {
                Title = titles[new Random().Next(titles.Length)],
                Description = descriptions[new Random().Next(descriptions.Length)],
                State = bulkAddTasksDto.RandomizeStates ? (TaskState)new Random().Next(0, 3) : TaskState.NotStarted,
                DueDate = bulkAddTasksDto.RandomizeDueDates ? DateOnly.FromDateTime(DateTime.Now.AddDays(new Random().Next(1, 30))) : null,
            };

            if (bulkAddTasksDto.RandomizeTags)
            {
                var randomTagsCount = new Random().Next(1, 4); // Randomly select 1 to 3 tags
                task.Tags = Enumerable.Range(0, randomTagsCount)
                    .Select(_ => _tags[new Random().Next(_tags.Length)])
                    .Distinct()
                    .ToArray();
            }
            else
            {
                task.Tags = Array.Empty<string>();
            }

            task.ProjectId = new Random().Next(0, projects.Items.Count());

            task.CreatedAt = DateOnly.FromDateTime(DateTime.Now);
            task.LastModifiedAt = DateOnly.FromDateTime(DateTime.Now);
            if (task.State == TaskState.Completed)
            {
                task.CompletedAt = DateOnly.FromDateTime(DateTime.Now);
            }
            else
            {
                task.CompletedAt = null;
            }
            tasks.Add(task);
        }
        _context.Tasks.AddRange(tasks);
        await _context.SaveChangesAsync();
        return tasks;
    }
}