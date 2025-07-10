using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TodoListApi.Application.Dtos;
using TodoListApi.Application.Services;
using TodoListApi.Domain;

public static class ProjectEndpoints
{
    public static IEndpointRouteBuilder MapProjectEndpoints(this IEndpointRouteBuilder app)
    {
        var projects = app.MapGroup("/projects");
        projects.MapGet("/", async (IProjectService service, [AsParameters] ProjectQueryParams projectQueryParams) =>
        {
            var projects = await service.GetAllProjectsAsync(projectQueryParams);
            return projects is not null ? Results.Ok(projects) : Results.NotFound();
        });

        projects.MapPost("/", async (IProjectService service, ProjectItemDto projectItemDto, IMapper mapper) =>
        {  
                var projectItem = mapper.Map<Project>(projectItemDto);
                var created = await service.AddProjectAsync(projectItem);
                return Results.Created($"/tasks/{created.Id}", created);
        });

        projects.MapGet("/{id:int}", async (IProjectService service, int id) =>
        {
            var project = await service.GetProjectByIdAsync(id);
            return project is not null ? Results.Ok(project) : Results.NotFound();
        });

        projects.MapPut("/{id:int}", async (IProjectService service, int id, ProjectItemDto projectItemDto, IMapper mapper) =>
        {
            var projectItem = mapper.Map<Project>(projectItemDto);
            await service.UpdateProjectAsync(id, projectItem);
            return Results.NoContent();
        });

        projects.MapDelete("/{id:int}", async (IProjectService service, int id) =>
        {
            if (id <= 0)
            {
                return Results.BadRequest("Invalid project ID.");
            }
            await service.DeleteProjectAsync(id);
            return Results.NoContent();
        });

        projects.MapPost("/populate-database", async (IProjectService service, BulkAddProjectsDto dto) =>
        {
            if (dto.NumberOfProjects <= 0)
            {
                return Results.BadRequest("Number of projects must be greater than zero.");
            }

            var createdProjects = await service.PopulateDatabaseAsync(dto);
            return Results.Ok(createdProjects);
        });

        return app;
    }
}