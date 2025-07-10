namespace TodoListApi.Application.Dtos;

using System;
using TodoListApi.Types;
public class PatchTaskItemDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }

    public DateOnly? DueDate { get; set; } = null;

    public TaskState? State { get; set; } = null;

    public String[] Tags { get; set; } = Array.Empty<string>();
}