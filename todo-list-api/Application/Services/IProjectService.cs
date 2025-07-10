using System.Collections.Generic;
using System.Threading.Tasks;
using TodoListApi.Application.Dtos;
using TodoListApi.Domain;

namespace TodoListApi.Application.Services;

public interface IProjectService
{
    Task<Project> AddProjectAsync(Project project);
    Task<Project> GetProjectByIdAsync(int id);
    Task<IEnumerable<Project>> GetAllProjectsAsync();
    Task UpdateProjectAsync(int id, Project project);
    Task DeleteProjectAsync(int id);
    Task<List<Project>> PopulateDatabaseAsync(BulkAddProjectsDto dto);
}