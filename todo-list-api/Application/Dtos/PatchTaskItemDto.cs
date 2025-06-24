namespace TodoListApi.Application.Dtos;
using TodoListApi.Types;
public class PatchTaskItemDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public TaskState? State { get; set; } = null;
}