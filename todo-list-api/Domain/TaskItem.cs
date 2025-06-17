
using System;

namespace TodoListApi.Domain
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? CompletedAt { get; set; }

        public DateTime? DueDate { get; set; }
        public DateTime? LastModifiedAt { get; set; }
    }
}