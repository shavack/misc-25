using TodoListApi.Application.Services;
using Xunit;
using TodoListApi.Data;
using TodoListApi.Domain;
using Moq;
using Microsoft.EntityFrameworkCore;

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
            await service.AddTaskAsync(new TaskItem { Title = $"Task {i + 1}", Description = $"Description {i + 1}"});
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
            await service.AddTaskAsync(new TaskItem { Title = $"Task {i + 1}", Description = $"Description {i + 1}", IsCompleted = i%2 == 0 });    
        }

        var tasks = await service.GetAllTasksAsync(isCompleted: false);

        Assert.Equal(2, tasks.Items.Count());
        Assert.Equal(5, tasks.TotalCount);
        Assert.Equal(1, tasks.Page);
        Assert.Equal(10, tasks.PageSize);
        Assert.Equal(1, tasks.TotalPages);
    }
}