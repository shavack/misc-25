using System;
using System.ComponentModel.DataAnnotations;
using TodoListApi.Types;

namespace TodoListApi.Application.Dtos
{
    public class TaskItemDto
    {
        [Required]
        public required string Title { get; set; }
        public TaskState State { get; set; }
        public string Description { get; set; }
        public DateOnly? CompletedAt { get; set; }
        public DateOnly? DueDate { get; set; }
        public String[] Tags { get; set; } = Array.Empty<string>(); 

        public int ProjectId { get; set; } = 0;
    }
}