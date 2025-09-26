using B08C14_InventoryManagement.Data;
using B08C14_InventoryManagement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
namespace B08C14_InventoryManagement.Controllers
{
   

    [Authorize(Roles = "Admin")]
    public class SuppliersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SuppliersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Suppliers
        public async Task<IActionResult> Index()
        {
            var suppliers = await _context.Suppliers
                .Include(s => s.User)
                .ToListAsync();
            return View(suppliers);
        }

        // AJAX: Get single supplier
        [HttpGet]
        public async Task<IActionResult> GetSupplier(int id)
        {
            var supplier = await _context.Suppliers
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (supplier == null) return Json(new { success = false, message = "Supplier not found." });
            return Json(new { success = true, data = supplier });
        }

        // AJAX: Create
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Name,Email,Mobile,Address")] Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                supplier.CreatedAt = DateTime.Now;
                supplier.CreatedBy = User.Identity?.Name ?? "Admin";
                supplier.IsActive = true;

                var user = new ApplicationUser
                {
                    UserName = supplier.Email,
                    Email = supplier.Email,
                    Name = supplier.Name,
                    PhoneNo = supplier.Mobile
                };
                var result = await _userManager.CreateAsync(user, "DefaultPassword123!");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Supplier");
                    supplier.UserId = user.Id;
                }
                else
                {
                    return Json(new { success = false, message = string.Join(", ", result.Errors.Select(e => e.Description)) });
                }

                _context.Suppliers.Add(supplier);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Supplier created successfully." });
            }
            return Json(new { success = false, message = "Validation failed." });
        }

        // AJAX: Edit
        [HttpPost]
        public async Task<IActionResult> Edit([Bind("Id,Name,Email,Mobile,Address")] Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                var existing = await _context.Suppliers.FindAsync(supplier.Id);
                if (existing == null) return Json(new { success = false, message = "Supplier not found." });

                existing.Name = supplier.Name;
                existing.Email = supplier.Email;
                existing.Mobile = supplier.Mobile;
                existing.Address = supplier.Address;
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

                _context.Suppliers.Update(existing);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Supplier updated successfully." });
            }
            return Json(new { success = false, message = "Validation failed." });
        }

        // AJAX: Delete
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null) return Json(new { success = false, message = "Supplier not found." });

            if (!string.IsNullOrEmpty(supplier.UserId))
            {
                var user = await _userManager.FindByIdAsync(supplier.UserId);
                if (user != null)
                {
                    await _userManager.DeleteAsync(user);
                }
            }

            _context.Suppliers.Remove(supplier);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Supplier deleted successfully." });
        }
    }

}