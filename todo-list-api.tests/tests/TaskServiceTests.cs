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

        Assert.Empty(tasks);
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
        Assert.Single(tasks);
        Assert.Equal("Test Task", tasks.ToList()[0].Title);
    }
}