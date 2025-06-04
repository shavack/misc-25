using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TodoListApi.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using TodoListApi.DTOs;
using TodoListApi.Services;
using TodoListApi.Models;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<TodoContext>(options =>
    options.UseInMemoryDatabase("TodoListDb"));
builder.Services.AddControllers();

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Enable Swagger middleware
if (app.Environment.IsDevelopment() || app.Environment.IsStaging() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/tasks", async (TaskItemDto dto, ITaskService service) =>
{
    var taskItem = new TaskItem
    {
        Title = dto.Title,
        IsDone = dto.IsCompleted
    };

    var created = await service.AddTaskAsync(taskItem);
    return Results.Created($"/tasks/{created.Id}", created);
});

app.MapPut("/tasks/{id}", async (int id, TaskItemDto dto, ITaskService service) =>
{
    var taskItem = new TaskItem
    {
        Id = id,
        Title = dto.Title,
        IsDone = dto.IsCompleted
    };

    await service.UpdateTaskAsync(id, taskItem);
    return Results.NoContent();
});


// Configure the HTTP request pipeline.
app.UseAuthorization();

app.MapControllers();

app.Run();