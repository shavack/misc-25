using Microsoft.AspNetCore.Routing;
using TodoListApi.Domain;
using TodoListApi.Application.Services;
using TodoListApi.Application.Dtos;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using FluentValidation;

public static class TaskEndpoints
{
    public static IEndpointRouteBuilder MapTaskEndpoints(this IEndpointRouteBuilder app)
    {
        var tasks = app.MapGroup("/tasks");
        tasks.MapGet("/", async (ITaskService service, [AsParameters] TaskQueryParams taskQueryParams, IValidator<TaskQueryParams> validator) =>
        {
            validator.ValidateAndThrow(taskQueryParams);
            var taskItems = await service.GetAllTasksAsync(taskQueryParams.Page, taskQueryParams.PageSize, taskQueryParams.Sort, taskQueryParams.IsCompleted);
            return Results.Ok(taskItems);
        });

        tasks.MapGet("/{id}", async (int id, ITaskService service) =>
        {
            var taskItem = await service.GetTaskByIdAsync(id);
            if (taskItem == null)
            {
                return Results.NotFound();
            }
            return Results.Ok(taskItem);
        });

        tasks.MapPost("/", async (TaskItemDto dto, ITaskService service, IMapper mapper, IValidator<TaskItemDto> validator) =>
        {
            validator.ValidateAndThrow(dto);

            var taskItem = mapper.Map<TaskItem>(dto);
            var created = await service.AddTaskAsync(taskItem);
            return Results.Created($"/tasks/{created.Id}", created);
        });

        tasks.MapPut("/{id}", async (int id, TaskItemDto dto, ITaskService service, IMapper mapper, IValidator<TaskItemDto> validator) =>
        {
            validator.ValidateAndThrow(dto);

            var taskItem = mapper.Map<TaskItem>(dto);
            await service.UpdateTaskAsync(id, taskItem);
            return Results.NoContent();
        });

        tasks.MapDelete("/{id}", async (int id, ITaskService service) =>
        {
            var toBeDeleted = await service.GetTaskByIdAsync(id);
            if (toBeDeleted == null)
            {
                return Results.NotFound();
            }
            await service.DeleteTaskAsync(id);
            return Results.NoContent();
        });
        tasks.MapGet("/stats", async (ITaskService service) =>
        {
            var stats = await service.GetStatisticsAsync();
            return Results.Ok(stats);
        });


        app.MapGet("error", () =>
        {
            return Results.Problem("You did something wrong, boi");
        });

        return app;
    }

}