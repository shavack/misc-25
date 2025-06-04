using System.ComponentModel.DataAnnotations;

namespace TodoListApi.DTOs
{
    public class TaskItemDto
    {
        [Required]
        public string Title { get; set; }
        public bool IsCompleted { get; set; }
    }
}