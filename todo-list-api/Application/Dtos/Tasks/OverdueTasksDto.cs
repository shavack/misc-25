namespace TodoListApi.Application.Dtos
{
    /// <summary>
    /// Represents a data transfer object for overdue tasks.
    /// </summary>
    public class OverdueTasksDto
    {
        public int? Page { get; set; } = 1;
        public int? PageSize { get; set; } = 10;
    }
}