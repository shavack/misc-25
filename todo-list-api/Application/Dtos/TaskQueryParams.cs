namespace TodoListApi.Application.Dtos;

public class TaskQueryParams
{
    public int? Page { get; set; } = 1;
    public int? PageSize { get; set; } = 10;
    public string Sort { get; set; } = "asc";
    public bool? IsCompleted { get; set; } = false;
    public string Title { get; set; }
}