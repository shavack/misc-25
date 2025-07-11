using System.Collections.Generic;

namespace TodoListApi.Application.Dtos
{
    public class TasksInProjectsQueryParams
    {
        public int[] ProjectIds { get; set; }
        public int? Page { get; set; } = 1;
        public int? PageSize { get; set; } = 10;
    }
}