using TodoListApi.Application.Services;
using Xunit;
using TodoListApi.Data;
using TodoListApi.Domain;
using Moq;
using Microsoft.EntityFrameworkCore;
using TodoListApi.Application.Dtos;
using System.Threading.Tasks;

public class TaskServiceTests
{
    [Fact]
    public async Task GetAllTasks_ReturnsEmptyList()
    {
        using var context = CreateInMemoryDbContext();
        var service = new TaskService(context);
        var tasksQueryParams = new TaskQueryParams();
        var tasks = await service.GetAllTasksAsync(tasksQueryParams);

        Assert.Empty(tasks.Items);
        Assert.Equal(1, tasks.Page);
        Assert.Equal(10, tasks.PageSize);
        Assert.Equal(0, tasks.TotalCount);
        Assert.Equal(0, tasks.TotalPages);
    }

    [Fact]
    public async Task GetTaskByIdAsync_ReturnsTask()
    {
        using var context = CreateInMemoryDbContext();
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
        using var context = CreateInMemoryDbContext();
        var service = new TaskService(context);

        var task = new TaskItem { Title = "Test Task", Description = "This is a test task." };
        await service.AddTaskAsync(task);
        var tasksQueryParams = new TaskQueryParams();

        var tasks = await service.GetAllTasksAsync(tasksQueryParams);
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
        using var context = CreateInMemoryDbContext();
        var service = new TaskService(context);

        await PopulateDatabase(service);

        var tasksQueryParams = new TaskQueryParams();
        var tasks = await service.GetAllTasksAsync(tasksQueryParams);

        Assert.Equal(5, tasks.Items.Count());
        Assert.Equal(5, tasks.TotalCount);
        Assert.Equal(1, tasks.Page);
        Assert.Equal(10, tasks.PageSize);
        Assert.Equal(1, tasks.TotalPages);
    }

    [Fact]
    public async Task GetAllTasks_MultipleTasksWithParameter()
    {
        using var context = CreateInMemoryDbContext();
        var service = new TaskService(context);

        await PopulateDatabase(service);
        var taskQueryParams = new TaskQueryParams
        {
            IsCompleted = false,
        };

        var tasks = await service.GetAllTasksAsync(taskQueryParams);

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

        await PopulateDatabase(service);

        var taskQueryParams = new TaskQueryParams
        {
            IsCompleted = true,
            Page = 1,
            PageSize = 10
        };
        var completedTasks = await service.GetAllTasksAsync(taskQueryParams);

        Assert.Equal(3, completedTasks.Items.Count());
        Assert.All(completedTasks.Items, task => Assert.True(task.IsCompleted));
    }

    [Fact]
    public async Task DeleteTaskAsync_RemovesTask()
    {
        using var context = CreateInMemoryDbContext();
        var service = new TaskService(context);

        await PopulateDatabase(service);

        await service.DeleteTasksAsync(new DeleteTasksDto
        {
            TaskIds = [1, 3]
        });
        var tasksQueryParams = new TaskQueryParams();

        var remainingTasks = await service.GetAllTasksAsync(tasksQueryParams);

        Assert.Equal(3, remainingTasks.TotalCount);
        Assert.DoesNotContain(remainingTasks.Items, task => task.Id == 1);
        Assert.DoesNotContain(remainingTasks.Items, task => task.Id == 3);
    }

    [Fact]
    public async Task DeleteTaskAsync_IdsDoesntExist()
    {
        using var context = CreateInMemoryDbContext();
        var service = new TaskService(context);
        await PopulateDatabase(service);

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

        var overdueTasksDto = new OverdueTasksDto
        {
            Page = 1,
            PageSize = 10
        };

        var overdueTasks = await service.GetOverdueTasksAsync(overdueTasksDto);
        Assert.Equal(5, overdueTasks.Count);
        Assert.All(overdueTasks, task => Assert.True(task.DueDate < DateTime.Now && !task.IsCompleted));
    }

    [Fact]
    public async Task ToggleCompletionAsync_TogglesTaskCompletion()
    {
        using var context = CreateInMemoryDbContext();
        var service = new TaskService(context);

        var task = new TaskItem { Title = "Test Task", Description = "This is a test task." };
        var addedTask = await service.AddTaskAsync(task);

        // Initially, the task should not be completed
        Assert.False(addedTask.IsCompleted);

        // Toggle completion
        await service.ToggleCompletionAsync(addedTask.Id);

        // Now, the task should be completed
        var toggledTask = await service.GetTaskByIdAsync(addedTask.Id);
        Assert.True(toggledTask.IsCompleted);

        // Toggle again to mark it as incomplete
        await service.ToggleCompletionAsync(toggledTask.Id);

        // Now, the task should not be completed again
        var finalTask = await service.GetTaskByIdAsync(toggledTask.Id);
        Assert.False(finalTask.IsCompleted);
    }

    [Fact]
    public async Task GetAllTasksAsync_FilterWithDueDate()
    {
        using var context = CreateInMemoryDbContext();
        var service = new TaskService(context);

        await PopulateDatabase(service);

        var taskQueryParams = new TaskQueryParams
        {
            DueDateFrom = DateTime.Now.AddDays(1),
            DueDateTo = DateTime.Now.AddDays(3),
        };

        var tasks = await service.GetAllTasksAsync(taskQueryParams);

        Assert.Equal(2, tasks.Items.Count());
        Assert.All(tasks.Items, task =>
        {
            Assert.NotNull(task.DueDate);
            Assert.InRange(task.DueDate.Value, DateTime.Now.AddDays(1), DateTime.Now.AddDays(3));
        });
    }

    [Fact]
    public async Task GetAllTasksAsync_SortByDueDate()
    {
        using var context = CreateInMemoryDbContext();
        var service = new TaskService(context);

        await PopulateDatabase(service, 5);

        var taskQueryParams = new TaskQueryParams
        {
            SortBy = "dueDate",
            SortOrder = "desc"
        };

        var tasks = await service.GetAllTasksAsync(taskQueryParams);

        Assert.Equal(5, tasks.Items.Count());
        Assert.Equal("Task 1", tasks.Items.Last().Title);
        Assert.Equal("Task 5", tasks.Items.First().Title);
    }

    private static AppDbContext CreateInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    private static async Task PopulateDatabase(TaskService taskService, int numberOfTasks = 5)
    {
        for (int i = 0; i < numberOfTasks; i++)
        {
            await taskService.AddTaskAsync(new TaskItem
            {
                Title = $"Task {i + 1}",
                Description = $"Description {i + 1}",
                IsCompleted = i % 2 == 0,
                DueDate = DateTime.Now.AddDays(i)
            });
        }
    }
}