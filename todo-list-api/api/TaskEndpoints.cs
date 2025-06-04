using Microsoft.AspNetCore.Routing;
using TodoListApi.DTOs;
using Microsoft.AspNetCore.Builder;
using TodoListApi.Models;
using Microsoft.AspNetCore.Http;

public static class TaskEndpoints
{
    public static IEndpointRouteBuilder MapTaskEndpoints(this IEndpointRouteBuilder app)
    {

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

        return app;
    }

}