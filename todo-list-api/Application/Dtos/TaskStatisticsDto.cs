using System.ComponentModel.DataAnnotations;

namespace TodoListApi.Application.Dtos
{
    public class TaskStatisticsDto
    {
        [Required]
        public int TotalTasks { get; set; }
        [Required]
        public int CompletedTasks { get; set; }
        [Required]
        public int PendingTasks { get; set; }
    }
}