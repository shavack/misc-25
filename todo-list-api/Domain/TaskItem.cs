
using System;

namespace TodoListApi.Domain
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateOnly CreatedAt { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public DateOnly? CompletedAt { get; set; }

        public DateOnly? DueDate { get; set; }
        public DateOnly? LastModifiedAt { get; set; }
    }
}