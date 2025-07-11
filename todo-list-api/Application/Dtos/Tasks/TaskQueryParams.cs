using System;
using TodoListApi.Types;

namespace TodoListApi.Application.Dtos;

public class TaskQueryParams
{
    public int? Page { get; set; } = 1;
    public int? PageSize { get; set; } = 10;
    public string SortOrder { get; set; } = "asc";
    public string SortBy { get; set; } = "title";
    public TaskState? State { get; set; } = null;
    public string Title { get; set; }
    public DateOnly? FromDate { get; set; }
    public DateOnly? ToDate { get; set; }
    public DateOnly? DueDateFrom { get; set; }
    public DateOnly? DueDateTo { get; set; }
    public string[] Tags { get; set; } = Array.Empty<string>();
}