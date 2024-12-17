using System.Data.Entity;


namespace CFapiMigration.Models
{
    public class ContextFile : DbContext
    {
        public ContextFile() : base("name=DefaultConnection")
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
    }
}