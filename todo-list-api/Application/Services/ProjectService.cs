using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TodoListApi.Application.Dtos;
using TodoListApi.Data;
using TodoListApi.Domain;

namespace TodoListApi.Application.Services;

public class ProjectService : IProjectService
{
    private readonly AppDbContext _context;
    private readonly string[] _names = [
        "Project Alpha",
        "Project Beta",
        "Project Gamma",
        "Project Delta",
        "Project Epsilon",
        "Project Zeta",
        "Project Eta",
        "Project Theta",
        "Project Iota",
        "Project Kappa",
        "Project Lambda",
        "Project Mu",
    ];

    private readonly string[] _descriptions = [
        "Very important project",
        "This project is crucial for our success",
        "A project that will change the world",
        "A project that will revolutionize the industry",
        "A project that will make us rich",
        "A project that will make us famous",
    ];

    public ProjectService(AppDbContext dbContext)
    {
        _context = dbContext;
    }

    public async Task<Project> AddProjectAsync(Project project, int userId)
    {
        project.UserId = userId; // Set the UserId from the authenticated user
        _context.Projects.Add(project);
        await _context.SaveChangesAsync();
        return project;
    }

    public async Task<Project> GetProjectByIdAsync(int id)
    {
        return await _context.Projects.FindAsync(id);
    }

    public async Task<PaginatedResultDto<Project>> GetAllProjectsAsync(ProjectQueryParams projectQueryParams, int userId)
    {
        var page = projectQueryParams.Page;
        var pageSize = projectQueryParams.PageSize;
        if (page is null or < 1) page = 1;
        if (pageSize is null or < 1) pageSize = 10;

        var query = _context.Projects.AsQueryable();

        if (page.HasValue && pageSize.HasValue)
        {
            query = query.Skip((page.Value - 1) * pageSize.Value)
                         .Take(pageSize.Value);
        }
        var resultPaginated = new PaginatedResultDto<Project>
        {
            Page = page.Value,
            PageSize = page.Value,
            TotalCount = await _context.Tasks.CountAsync(),
            TotalPages = (int)Math.Ceiling((double)await _context.Tasks.CountAsync() / pageSize.Value),
            Items = await query.Where(p => p.UserId == userId).ToListAsync(),
        };
        return resultPaginated;
    }

    public async Task UpdateProjectAsync(int id, Project project)
    {
        if (id != project.Id)
        {
            throw new ArgumentException("Task ID mismatch");
        }
        _context.Entry(project).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteProjectAsync(int id)
    {
        var project = await _context.Projects.FindAsync(id) ?? throw new KeyNotFoundException("Project not found");
        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();
    }
    
    public async Task<List<Project>> PopulateDatabaseAsync(int numberOfProjects)
    {
        var projects = new List<Project>();
        var users = await _context.Users.ToListAsync();
        for (int i = 0; i < numberOfProjects; i++)
        {
            var project = new Project
            {
                Name = _names[i % _names.Length],
                Description = _descriptions[Random.Shared.Next(_descriptions.Length)],
                CreatedAt = DateOnly.FromDateTime(DateTime.Now),
                LastModifiedAt = null,
                UserId = users[Random.Shared.Next(users.Count)].Id
            };
            projects.Add(project);
        }
        _context.Projects.AddRange(projects);
        await _context.SaveChangesAsync();
        return projects;
    }
}