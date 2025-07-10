namespace TodoListApi.Application.Dtos;

public class ProjectItemDto
{
    /// <summary>
    /// Gets or sets the name of the project.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the project.
    /// </summary>
    public string Description { get; set; } = string.Empty;
}