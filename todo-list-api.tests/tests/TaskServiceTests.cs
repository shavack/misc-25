using TodoListApi.Application.Services;
using Xunit;
using TodoListApi.Data;
using TodoListApi.Domain;
using Moq;
using Microsoft.EntityFrameworkCore;
using TodoListApi.Application.Dtos;

public class TaskServiceTests
{
    [Fact]
    public async Task GetAllTasks_ReturnsEmptyList()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new AppDbContext(options);
        var service = new TaskService(context);

        var tasks = await service.GetAllTasksAsync();

        Assert.Empty(tasks.Items);
        Assert.Equal(1, tasks.Page);
        Assert.Equal(10, tasks.PageSize);
        Assert.Equal(0, tasks.TotalCount);
        Assert.Equal(0, tasks.TotalPages);
    }

    [Fact]
    public async Task GetTaskByIdAsync_ReturnsTask()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        using var context = new AppDbContext(options);
        var service = new TaskService(context);
        var task = new TaskItem { Title = "Test Task", Description = "This is a test task." };
        var response = await service.AddTaskAsync(task);
        var addedTask = await service.GetTaskByIdAsync(response.Id);
        Assert.NotNull(addedTask);
        Assert.Equal("Test Task", addedTask.Title);
        Assert.Equal("This is a test task.", addedTask.Description);
        Assert.Equal(response.Id, addedTask.Id);
    }

    [Fact]
    public async Task AddTask_AddsNewTask()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new AppDbContext(options);
        var service = new TaskService(context);

        var task = new TaskItem { Title = "Test Task", Description = "This is a test task." };
        await service.AddTaskAsync(task);

        var tasks = await service.GetAllTasksAsync();
        Assert.Single(tasks.Items);
        Assert.Equal("Test Task", tasks.Items.ToList()[0].Title);
        Assert.Equal("This is a test task.", tasks.Items.ToList()[0].Description);
        Assert.Equal(1, tasks.TotalCount);
        Assert.Equal(1, tasks.TotalPages);
        Assert.Equal(1, tasks.Page);
        Assert.Equal(10, tasks.PageSize);
    }

    [Fact]
    public async Task GetAllTasks_MultipleTasks()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new AppDbContext(options);
        var service = new TaskService(context);

        for (int i = 0; i < 5; i++)
        {
            await service.AddTaskAsync(new TaskItem { Title = $"Task {i + 1}", Description = $"Description {i + 1}" });
        }

        var tasks = await service.GetAllTasksAsync();

        Assert.Equal(5, tasks.Items.Count());
        Assert.Equal(5, tasks.TotalCount);
        Assert.Equal(1, tasks.Page);
        Assert.Equal(10, tasks.PageSize);
        Assert.Equal(1, tasks.TotalPages);
    }

    [Fact]
    public async Task GetAllTasks_MultipleTasksWithParameter()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new AppDbContext(options);
        var service = new TaskService(context);

        for (int i = 0; i < 5; i++)
        {
            await service.AddTaskAsync(new TaskItem { Title = $"Task {i + 1}", Description = $"Description {i + 1}", IsCompleted = i % 2 == 0 });
        }

        var tasks = await service.GetAllTasksAsync(isCompleted: false);

        Assert.Equal(2, tasks.Items.Count());
        Assert.Equal(5, tasks.TotalCount);
        Assert.Equal(1, tasks.Page);
        Assert.Equal(10, tasks.PageSize);
        Assert.Equal(1, tasks.TotalPages);
    }

    [Fact]
    public async Task GetCompletedTasks_ReturnsCompletedTasks()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new AppDbContext(options);
        var service = new TaskService(context);

        for (int i = 0; i < 5; i++)
        {
            await service.AddTaskAsync(new TaskItem { Title = $"Task {i + 1}", Description = $"Description {i + 1}", IsCompleted = i % 2 == 0 });
        }

        var completedTasks = await service.GetAllTasksAsync(isCompleted: true);

        Assert.Equal(3, completedTasks.Items.Count());
        Assert.All(completedTasks.Items, task => Assert.True(task.IsCompleted));
    }

    [Fact]
    public async Task DeleteTaskAsync_RemovesTask()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new AppDbContext(options);
        var service = new TaskService(context);

        for (int i = 0; i < 5; i++)
        {
            await service.AddTaskAsync(new TaskItem { Title = $"Task {i + 1}", Description = $"Description {i + 1}", IsCompleted = i % 2 == 0 });
        }

        await service.DeleteTasksAsync(new DeleteTasksDto
        {
            TaskIds = [1, 3]
        });

        var remainingTasks = await service.GetAllTasksAsync();

        Assert.Equal(3, remainingTasks.TotalCount);
        Assert.DoesNotContain(remainingTasks.Items, task => task.Id == 1);
        Assert.DoesNotContain(remainingTasks.Items, task => task.Id == 3);
    }

    [Fact]
    public async Task DeleteTaskAsync_IdsDoesntExist()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new AppDbContext(options);
        var service = new TaskService(context);
        for (int i = 0; i < 5; i++)
        {
            await service.AddTaskAsync(new TaskItem { Title = $"Task {i + 1}", Description = $"Description {i + 1}", IsCompleted = i % 2 == 0 });
        }

        var deleteTasksDto = new DeleteTasksDto
        {
            TaskIds = [1, 2, 8]
        };

        await Assert.ThrowsAsync<KeyNotFoundException>(() => service.DeleteTasksAsync(deleteTasksDto));
    }

    [Fact]
    public async Task GetOverdueTasks_ReturnsValues()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new AppDbContext(options);
        var service = new TaskService(context);

        for (int i = 0; i < 5; i++)
        {
            await service.AddTaskAsync(new TaskItem { Title = $"Task {i + 1}", Description = $"Description {i + 1}", IsCompleted = false, DueDate = DateTime.Now.AddDays(-1) });
        }
        for (int i = 0; i < 5; i++)
        {
            await service.AddTaskAsync(new TaskItem { Title = $"Task {i + 6}", Description = $"Description {i + 6}", IsCompleted = true, DueDate = DateTime.Now.AddDays(1) });
        }

        var overdueTasks = await service.GetOverdueTasksAsync();
        Assert.Equal(5, overdueTasks.Count);
        Assert.All(overdueTasks, task => Assert.True(task.DueDate < DateTime.Now && !task.IsCompleted));
    }
    
}