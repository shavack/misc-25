using System;
namespace TodoListApi.Application.Dtos;
public class DeleteTasksDto
{
    /// <summary>
    /// Gets or sets the IDs of the tasks to be deleted.
    /// </summary>
    public int[] TaskIds { get; set; } = Array.Empty<int>();
}