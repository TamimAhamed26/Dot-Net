using DAL.EF.Entities;
using System.Data.Entity;

namespace DAL.EF
{
    public class LayerContext : DbContext
    {
        public LayerContext()
            : base("name=DefaultConnection") // DefaultConnection should match your connection string in Web.config
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
