using System;
using System.ComponentModel.DataAnnotations;

namespace TodoListApi.Application.Dtos
{
    public class TaskItemDto
    {
        [Required]
        public required string Title { get; set; }
        public bool IsCompleted { get; set; }
        public string Description { get; set; }
        public DateOnly? CompletedAt { get; set; }
        public DateOnly? DueDate { get; set; } 
    }
}