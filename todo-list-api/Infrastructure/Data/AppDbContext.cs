using Microsoft.EntityFrameworkCore;
using TodoListApi.Domain;

namespace TodoListApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<TaskItem> Tasks { get; set; }

    }
}