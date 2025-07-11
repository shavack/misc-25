namespace TodoListApi.Application.Dtos;

public class BulkAddProjectsDto
{
    /// <summary>
    /// Gets or sets the number of projects to add.
    /// </summary>
    public required int NumberOfProjects { get; set; }
}