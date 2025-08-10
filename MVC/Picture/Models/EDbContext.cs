using Microsoft.EntityFrameworkCore;

namespace PictureU.Models
{
    public class EDbContext : DbContext
    {
        public EDbContext(DbContextOptions<EDbContext> options) : base(options)
        {
        }
        public DbSet<Employee> Employees { get; set; }
    }
}
