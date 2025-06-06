using System.ComponentModel.DataAnnotations;

namespace TodoListApi.Application.Dtos
{
    public class TaskItemDto
    {
        [Required]
        public string Title { get; set; }
        public bool IsCompleted { get; set; }
    }
}