using Microsoft.EntityFrameworkCore;
using TodoListApi.Models;
namespace TodoListApi.Data
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options) : base(options) { }

        public DbSet<TaskItem> Tasks { get; set; }
    }
}