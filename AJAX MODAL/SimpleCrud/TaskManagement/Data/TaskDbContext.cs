using Microsoft.EntityFrameworkCore;
using TaskManagement.Models;

namespace TaskManagement.Data;

public class TaskDbContext:DbContext
{
    public TaskDbContext(DbContextOptions<TaskDbContext> options): base(options)
    {

    }
    public DbSet<TaskList> Tasks { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<AssignedTask> AssignedTasks { get; set; }
}
