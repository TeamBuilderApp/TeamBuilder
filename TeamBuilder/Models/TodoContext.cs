using Microsoft.EntityFrameworkCore;

namespace TeamBuilder.Models
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options)
            : base(options)
        {
        }

        public DbSet<TodoItem> TeamBuilder { get; set; } = null!;
    }
}
