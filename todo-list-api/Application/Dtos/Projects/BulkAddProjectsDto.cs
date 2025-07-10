namespace TodoListApi.Application.Dtos;

public class BulkAddProjectsDto
{
    /// <summary>
    /// Gets or sets the number of projects to add.
    /// </summary>
    public required int NumberOfProjects { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to randomize project names.
    /// </summary>
    public required bool RandomizeNames { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether to randomize project descriptions.
    /// </summary>
    public required bool RandomizeDescriptions { get; set; } = true;
}