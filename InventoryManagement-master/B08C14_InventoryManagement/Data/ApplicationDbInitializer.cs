using B08C14_InventoryManagement.Models;
using Microsoft.AspNetCore.Identity;

namespace B08C14_InventoryManagement.Data
{
    public static class ApplicationDbInitializer
    {
        public static async Task SeedRolesAndUsersAsync(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            string[] roleNames = { "Admin", "Supplier", "Customer", "Dealer" };

            // Seed Roles
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                    await roleManager.CreateAsync(new IdentityRole(roleName));
            }

            // Seed Users
            await CreateUserWithRole(userManager, context, "admin@example.com", "Admin123!", "Admin");
            await CreateUserWithRole(userManager, context, "admin1@example.com", "Admin123!", "Admin");

            await CreateUserWithRole(userManager, context, "supplier@example.com", "Supplier123!", "Supplier");
            await CreateUserWithRole(userManager, context, "supplier1@example.com", "Supplier123!", "Supplier");
            await CreateUserWithRole(userManager, context, "supplier2@example.com", "Supplier123!", "Supplier");

            await CreateUserWithRole(userManager, context, "customer@example.com", "Customer123!", "Customer");
            await CreateUserWithRole(userManager, context, "customer1@example.com", "Customer123!", "Customer");
            await CreateUserWithRole(userManager, context, "customer2@example.com", "Customer123!", "Customer");
        }

        private static async Task CreateUserWithRole(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context,
            string email, string password, string role)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user != null) return;

            var newUser = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                Name = role + " User",
                PhoneNo = "01234567890"
            };

            var result = await userManager.CreateAsync(newUser, password);
            if (!result.Succeeded)
                throw new Exception($"Failed to create {role} user: {string.Join(", ", result.Errors.Select(e => e.Description))}");

            await userManager.AddToRoleAsync(newUser, role);

            if (role == "Customer")
            {
                var customer = new Customer
                {
                    Name = newUser.Name,
                    Email = newUser.Email,
                    Mobile = newUser.PhoneNo,
                    UserId = newUser.Id,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "Seed",
                    IsActive = true
                };
                context.Customers.Add(customer);
            }

            if (role == "Supplier")
            {
                var supplier = new Supplier
                {
                    Name = newUser.Name,
                    Email = newUser.Email,
                    Mobile = newUser.PhoneNo,
                    UserId = newUser.Id,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "Seed",
                    IsActive = true
                };
                context.Suppliers.Add(supplier);
            }

            await context.SaveChangesAsync();
        }
    }
}
