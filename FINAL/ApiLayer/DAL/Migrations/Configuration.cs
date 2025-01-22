namespace DAL.Migrations
{
    using DAL.EF.Entities;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DAL.EF.LayerContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DAL.EF.LayerContext context)
        {
            context.Customers.AddOrUpdate(
                 c => c.CustomerId, // Use CustomerId to avoid duplicate seeding
                 new Customer { CustomerId = 1, Name = "Rahim", Email = "rahim@gmail.com" },
                 new Customer { CustomerId = 2, Name = "Karim", Email = "karim@gmail.com" },
                 new Customer { CustomerId = 3, Name = "Salma", Email = "salma@gmail.com" },
                 new Customer { CustomerId = 4, Name = "Shamim", Email = "shamim@gmail.com" },
                 new Customer { CustomerId = 5, Name = "Ruma", Email = "ruma@gmail.com" }
             );

            // Seed Products
            context.Products.AddOrUpdate(
                p => p.ProductId, // Use ProductId to avoid duplicate seeding
                new Product { ProductId = 1, Name = "Rice", Price = 50.00m, CustomerId = 1 },
                new Product { ProductId = 2, Name = "Lentils", Price = 100.00m, CustomerId = 2 },
                new Product { ProductId = 3, Name = "Sugar", Price = 60.00m, CustomerId = 3 },
                new Product { ProductId = 4, Name = "Salt", Price = 20.00m, CustomerId = 4 },
                new Product { ProductId = 5, Name = "Tea", Price = 200.00m, CustomerId = 5 }
            );

            // Save changes
            context.SaveChanges();
        }
    }
}
