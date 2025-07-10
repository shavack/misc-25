using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoListApi.Application.Dtos;
using TodoListApi.Data;
using TodoListApi.Domain;
using TodoListApi.Types;

namespace TodoListApi.Application.Services;

public class ProjectService : IProjectService
{
    private readonly AppDbContext _context;

    private readonly string[] _names = new[]{
        "Project Alpha",
        "Project Beta",
        "Project Gamma",
        "Project Delta",
        "Project Epsilon"
    };

    private readonly string[] _descriptions = new[]{
        "Very important project",
        "This project is crucial for our success",
        "A project that will change the world",
        "A project that will revolutionize the industry",
        "A project that will make us rich",
        "A project that will make us famous",
    };  

    public ProjectService(AppDbContext dbContext)
    {
        _context = dbContext;
    }

    public async Task<Project> AddProjectAsync(Project project)
    {
        _context.Projects.Add(project);
        await _context.SaveChangesAsync();
        return project;
    }

    public async Task<Project> GetProjectByIdAsync(int id)
    {
        return await _context.Projects.FindAsync(id);
    }

    public async Task<IEnumerable<Project>> GetAllProjectsAsync()
    {
        return await _context.Projects.ToListAsync();
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
    
    public async Task<List<Project>> PopulateDatabaseAsync(BulkAddProjectsDto dto)
    {
        var projects = new List<Project>();
        for (int i = 0; i < dto.NumberOfProjects; i++)
        {
            var project = new Project
            {
                Name = dto.RandomizeNames ? 
                    _names[Random.Shared.Next(_names.Length)] : 
                    $"Project {i + 1}",
                Description = dto.RandomizeDescriptions ? 
                    _descriptions[Random.Shared.Next(_descriptions.Length)] : 
                    $"Description for Project {i + 1}"
            };
            projects.Add(project);
        }
        _context.Projects.AddRange(projects);
        await _context.SaveChangesAsync();
        return projects;
    }
}