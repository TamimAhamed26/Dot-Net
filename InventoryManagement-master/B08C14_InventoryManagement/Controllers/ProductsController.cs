using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using B08C14_InventoryManagement.Data;
using Microsoft.AspNetCore.Authorization;

namespace B08C14_InventoryManagement.Controllers
{


    [Authorize(Roles = "Admin")]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .ToListAsync();

            // Populate ViewBag for modal dropdowns
            ViewBag.Categories = await _context.Categories.OrderBy(c => c.Name).ToListAsync();
            ViewBag.Suppliers = await _context.Suppliers.OrderBy(s => s.Name).ToListAsync();

            return View(products);
        }


        // AJAX: Get single product
        [HttpGet]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _context.Products
                .AsNoTracking()
                .Where(p => p.Id == id)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Description,
                    p.Price,
                    p.StockQuantity,
                    p.CategoryId,
                    p.SupplierId
                })
                .FirstOrDefaultAsync();

            if (product == null)
                return Json(new { success = false, message = "Product not found." });

            return Json(new { success = true, data = product });
        }

        // AJAX: Create
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Name,Description,Price,StockQuantity,CategoryId,SupplierId")] Product product)
        {
            if (ModelState.IsValid)
            {
                product.CreatedAt = DateTime.Now;
                product.CreatedBy = User.Identity.Name ?? "Admin";
                product.IsActive = true;

                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Product created successfully." });
            }
            return Json(new { success = false, message = "Validation failed." });
        }

        // AJAX: Edit
        [HttpPost]
        public async Task<IActionResult> Edit([Bind("Id,Name,Description,Price,StockQuantity,CategoryId,SupplierId")] Product product)
        {
            if (ModelState.IsValid)
            {
                var existing = await _context.Products.FindAsync(product.Id);
                if (existing == null) return Json(new { success = false, message = "Product not found." });

                existing.Name = product.Name;
                existing.Description = product.Description;
                existing.Price = product.Price;
                existing.StockQuantity = product.StockQuantity;
                existing.CategoryId = product.CategoryId;
                existing.SupplierId = product.SupplierId;
                existing.UpdatedAt = DateTime.Now;
                existing.UpdatedBy = User.Identity.Name ?? "Admin";

                _context.Products.Update(existing);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Product updated successfully." });
            }
            return Json(new { success = false, message = "Validation failed." });
        }

        // AJAX: Delete
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return Json(new { success = false, message = "Product not found." });

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Product deleted successfully." });
        }
    }
}