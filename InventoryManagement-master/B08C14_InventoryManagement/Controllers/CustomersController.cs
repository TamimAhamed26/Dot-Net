using B08C14_InventoryManagement.Data;
using B08C14_InventoryManagement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
namespace B08C14_InventoryManagement.Controllers
{


    [Authorize(Roles = "Admin")]
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CustomersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {
            var customers = await _context.Customers
                .Include(c => c.User)
                .ToListAsync();
            return View(customers);
        }

        // AJAX: Get single customer
        [HttpGet]
        public async Task<IActionResult> GetCustomer(int id)
        {
            var customer = await _context.Customers
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (customer == null) return Json(new { success = false, message = "Customer not found." });
            return Json(new { success = true, data = customer });
        }

        // AJAX: Create
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Name,Email,Mobile,Address")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                customer.CreatedAt = DateTime.Now;
                customer.CreatedBy = User.Identity?.Name ?? "Admin";
                customer.IsActive = true;

                // Create linked ApplicationUser
                var user = new ApplicationUser
                {
                    UserName = customer.Email,
                    Email = customer.Email,
                    Name = customer.Name,
                    PhoneNo = customer.Mobile
                };

                var result = await _userManager.CreateAsync(user, "DefaultPassword123!");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Customer");
                    customer.UserId = user.Id;
                }
                else
                {
                    return Json(new { success = false, message = string.Join(", ", result.Errors.Select(e => e.Description)) });
                }

                _context.Add(customer);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Customer created successfully." });
            }
            return Json(new { success = false, message = "Validation failed." });
        }

        // AJAX: Edit
        [HttpPost]
        public async Task<IActionResult> Edit([Bind("Id,Name,Email,Mobile,Address")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                var existing = await _context.Customers.FindAsync(customer.Id);
                if (existing == null) return Json(new { success = false, message = "Customer not found." });

                existing.Name = customer.Name;
                existing.Email = customer.Email;
                existing.Mobile = customer.Mobile;
                existing.Address = customer.Address;
                existing.UpdatedAt = DateTime.Now;
                existing.UpdatedBy = User.Identity?.Name ?? "Admin";

                // update linked ApplicationUser
                if (!string.IsNullOrEmpty(existing.UserId))
                {
                    var user = await _userManager.FindByIdAsync(existing.UserId);
                    if (user != null)
                    {
                        user.Name = existing.Name;
                        user.Email = existing.Email;
                        user.UserName = existing.Email;
                        user.PhoneNo = existing.Mobile;
                        await _userManager.UpdateAsync(user);
                    }
                }

                _context.Customers.Update(existing);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Customer updated successfully." });
            }
            return Json(new { success = false, message = "Validation failed." });
        }

        // AJAX: Delete
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) return Json(new { success = false, message = "Customer not found." });

            if (!string.IsNullOrEmpty(customer.UserId))
            {
                var user = await _userManager.FindByIdAsync(customer.UserId);
                if (user != null)
                {
                    await _userManager.DeleteAsync(user);
                }
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Customer deleted successfully." });
        }
    }
}