using B08C14_InventoryManagement.Data;
using B08C14_InventoryManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace B08C14_InventoryManagement.Controllers
{
    public class OrderDetailsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Index
        public async Task<IActionResult> Index()
        {
            var orders = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .ToListAsync();

            ViewBag.Products = await _context.Products.ToListAsync();
            ViewBag.Customers = await _context.Customers.ToListAsync();
            return View(orders);
        }
        [HttpGet]
        public async Task<JsonResult> GetPrice(int productId)
        {
            var product = await _context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null)
                return Json(new { success = false, price = 0 });

            return Json(new { success = true, price = product.Price });
        }

        [HttpPost]
        public async Task<JsonResult> CreateOrder([FromForm] int customerId, [FromForm] DateTime orderDate, [FromForm] OrderDetails detail)
        {
            if (customerId == 0 || detail.ProductId == 0 || detail.Quantity <= 0)
            {
                return Json(new { success = false, message = "Invalid input" });
            }

            var order = new Order
            {
                CustomerId = customerId,
                OrderDate = orderDate,
                Status = OrderStatus.Pending,
                TotalAmount = 0
            };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            detail.OrderId = order.Id;
            detail.TotalPrice = detail.Quantity * detail.UnitPrice;
            _context.OrderDetails.Add(detail);
            await _context.SaveChangesAsync();

            order.TotalAmount = detail.TotalPrice;
            _context.Update(order);
            await _context.SaveChangesAsync();

            return Json(new { success = true, orderId = order.Id });
        }

        public async Task<JsonResult> GetById(int id)
        {
            var od = await _context.OrderDetails.FindAsync(id);
            if (od == null) return Json(null);

            return Json(new
            {
                id = od.Id,
                orderId = od.OrderId,
                productId = od.ProductId,
                quantity = od.Quantity,
                unitPrice = od.UnitPrice,
                totalPrice = od.TotalPrice
            });
        }

        [HttpPost]
        public async Task<JsonResult> Update([FromForm] OrderDetails model)
        {
            if (model.ProductId == 0 || model.Quantity <= 0)
            {
                return Json(new { success = false, message = "Invalid input" });
            }

            if (model.Id > 0)
            {
                var existing = await _context.OrderDetails.FindAsync(model.Id);
                if (existing == null) return Json(new { success = false, message = "Not found" });

                existing.ProductId = model.ProductId;
                existing.Quantity = model.Quantity;
                existing.UnitPrice = model.UnitPrice;
                existing.TotalPrice = model.Quantity * model.UnitPrice;
                _context.Update(existing);
            }
            else
            {
                if (model.OrderId <= 0)
                {
                    return Json(new { success = false, message = "Order ID required" });
                }
                model.TotalPrice = model.Quantity * model.UnitPrice;
                _context.Add(model);
            }

            await _context.SaveChangesAsync();

            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.Id == model.OrderId);

            if (order != null)
            {
                order.TotalAmount = order.OrderDetails.Sum(od => od.TotalPrice);
                _context.Update(order);
                await _context.SaveChangesAsync();
            }

            return Json(new { success = true });
        }

        // POST: Delete OrderDetail
        [HttpPost]
        public async Task<JsonResult> Delete(int id)
        {
            var od = await _context.OrderDetails.FindAsync(id);
            if (od != null)
            {
                int orderId = od.OrderId;
                _context.OrderDetails.Remove(od);
                await _context.SaveChangesAsync();

                var order = await _context.Orders
                    .Include(o => o.OrderDetails)
                    .FirstOrDefaultAsync(o => o.Id == orderId);
                if (order != null)
                {
                    order.TotalAmount = order.OrderDetails.Sum(od => od.TotalPrice);
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }

                return Json(new { success = true });
            }

            return Json(new { success = false, message = "Not found" });
        }
    }
}