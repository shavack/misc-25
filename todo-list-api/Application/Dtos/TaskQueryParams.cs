using System;

namespace TodoListApi.Application.Dtos;

public class TaskQueryParams
{
    public int? Page { get; set; } = 1;
    public int? PageSize { get; set; } = 10;
    public string SortOrder { get; set; } = "asc";
    public string SortBy { get; set; } = "title";
    public bool? IsCompleted { get; set; } = null;
    public string Title { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }

    public DateTime? DueDateFrom { get; set; }
    public DateTime? DueDateTo { get; set; }
}