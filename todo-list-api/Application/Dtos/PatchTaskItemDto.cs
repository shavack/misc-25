namespace TodoListApi.Application.Dtos;
public class PatchTaskItemDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool? IsCompleted { get; set; }
}