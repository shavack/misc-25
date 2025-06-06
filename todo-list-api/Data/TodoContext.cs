using Microsoft.EntityFrameworkCore;
using TodoListApi.Domain;
namespace TodoListApi.Data
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options) : base(options) { }

        public DbSet<TaskItem> Tasks { get; set; }
    }
}