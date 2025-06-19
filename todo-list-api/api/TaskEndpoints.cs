using Microsoft.AspNetCore.Routing;
using TodoListApi.Domain;
using TodoListApi.Application.Services;
using TodoListApi.Application.Dtos;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using FluentValidation;
using System.Threading.Tasks;

public static class TaskEndpoints
{
    public static IEndpointRouteBuilder MapTaskEndpoints(this IEndpointRouteBuilder app)
    {
        var tasks = app.MapGroup("/tasks");
        tasks.MapGet("/", async (ITaskService service, [AsParameters] TaskQueryParams taskQueryParams, IValidator<TaskQueryParams> validator) =>
        {
            validator.ValidateAndThrow(taskQueryParams);
            var taskItems = await service.GetAllTasksAsync(taskQueryParams.Page, taskQueryParams.PageSize, taskQueryParams.Sort, taskQueryParams.IsCompleted, taskQueryParams.Title);
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
            if(id <= 0)
            {
                return Results.BadRequest("Invalid task ID.");
            }
            await service.DeleteTaskAsync(id);
            return Results.NoContent();
        });

        tasks.MapGet("/stats", async ([AsParameters] StatisticsQueryParams statisticsQueryParams, ITaskService service) =>
        {
            if(statisticsQueryParams.FromDate.HasValue && statisticsQueryParams.ToDate.HasValue && statisticsQueryParams.FromDate > statisticsQueryParams.ToDate)
            {
                return Results.BadRequest("FromDate cannot be greater than ToDate.");
            }
            var stats = await service.GetStatisticsAsync(fromDate: statisticsQueryParams.FromDate, toDate: statisticsQueryParams.ToDate, title: statisticsQueryParams.Title, isCompleted: statisticsQueryParams.IsCompleted);
            return Results.Ok(stats);
        });

        tasks.MapGet("/completed", async ([AsParameters] TaskQueryParams taskQueryParams, ITaskService service) =>
        {
            var completedTasks = await service.GetAllTasksAsync(page: taskQueryParams.Page, pageSize: taskQueryParams.PageSize, isCompleted: true, sort: taskQueryParams.Sort, title: taskQueryParams.Title);
            return Results.Ok(completedTasks);
        });

        tasks.MapPatch("/{id}/complete", async (int id, ITaskService service) =>
        {
            if (id <= 0)
            {
                return Results.BadRequest("Invalid task ID.");
            }
            await service.SetCompleteAsync(id);
            return Results.NoContent();
        });

        tasks.MapPatch("{id}", async (PatchTaskItemDto patchTaskItemDto, ITaskService service) =>
        {
            if (patchTaskItemDto.Id <= 0)
            {
                return Results.BadRequest("Invalid task ID.");
            }
            await service.PatchTaskAsync(patchTaskItemDto);
            return Results.NoContent();
        });

        tasks.MapDelete("/delete", async ([AsParameters] DeleteTasksDto deleteTasksDto, ITaskService service) =>
        {
            if (deleteTasksDto.TaskIds == null || deleteTasksDto.TaskIds.Length == 0)
            {
                return Results.BadRequest("No task IDs provided for deletion.");
            }
            await service.DeleteTasksAsync(deleteTasksDto);
            return Results.NoContent();
        });
        
        tasks.MapGet("/overdue", async ([AsParameters] OverdueTasksDto overdueTasksDto, ITaskService service) =>
        {
            var overdueTasks = await service.GetOverdueTasksAsync(overdueTasksDto);

            return Results.Ok(overdueTasks);
        });

        app.MapGet("error", () =>
        {
            return Results.Problem("You did something wrong, boi");
        });

        app.MapGet("/", context =>
        {
            context.Response.Redirect("/swagger");
            return Task.CompletedTask;
        });

        return app;
    }

}