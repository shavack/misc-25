
using System;
using TodoListApi.Types;

namespace TodoListApi.Domain
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public TaskState? State { get; set; } = null;
        public DateOnly CreatedAt { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public DateOnly? CompletedAt { get; set; }

        public DateOnly? DueDate { get; set; }
        public DateOnly? LastModifiedAt { get; set; }
    }
}