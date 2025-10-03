using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using B08C14_InventoryManagement.Data;
using Microsoft.AspNetCore.Authorization;
namespace B08C14_InventoryManagement.Controllers
{


    [Authorize(Roles = "Admin")]
    public class CategotiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategotiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories.ToListAsync();
            return View(categories);
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Name,Description")] Categoty categoty)
        {
            if (ModelState.IsValid)
            {
                categoty.CreatedAt = DateTime.Now;
                categoty.CreatedBy = User.Identity.Name ?? "Admin";
                categoty.IsActive = true;

                _context.Add(categoty);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Category created successfully." });
            }

            return Json(new { success = false, message = "Validation failed." });
        }

        [HttpGet]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return Json(new { success = false, message = "Not found." });

            return Json(new { success = true, data = category });
        }

        [HttpPost]
        public async Task<IActionResult> Edit([Bind("Id,Name,Description")] Categoty categoty)
        {
            if (ModelState.IsValid)
            {
                var existing = await _context.Categories.FindAsync(categoty.Id);
                if (existing == null) return Json(new { success = false, message = "Category not found." });

                existing.Name = categoty.Name;
                existing.Description = categoty.Description;
                existing.UpdatedAt = DateTime.Now;
                existing.UpdatedBy = User.Identity.Name ?? "Admin";

                _context.Update(existing);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Category updated successfully." });
            }
            return Json(new { success = false, message = "Validation failed." });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return Json(new { success = false, message = "Category not found." });

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Category deleted successfully." });
        }
    }
}