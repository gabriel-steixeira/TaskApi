using Microsoft.EntityFrameworkCore;
using TaskApi.Data.Entities;

namespace TaskApi.Data
{
    public class TaskContext : DbContext
    {
        public TaskContext(DbContextOptions<TaskContext> options) : base (options)
        {
            
        }

        public DbSet<TaskClass> Tasks { get; set; }
    }
}
