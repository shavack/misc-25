using System;
using TodoListApi.Types;

namespace TodoListApi.Application.Dtos;

public class ProjectQueryParams
{
    public int? Page { get; set; } = 1;
    public int? PageSize { get; set; } = 10;
}