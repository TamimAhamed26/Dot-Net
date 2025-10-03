using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using B08C14_InventoryManagement.Data;

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
                return Json(new { success = false, message = "Invalid input" });

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
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

                await transaction.CommitAsync();
                return Json(new { success = true, orderId = order.Id });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return Json(new { success = false, message = "Transaction failed: " + ex.Message });
            }
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
                return Json(new { success = false, message = "Invalid input" });

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
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
                        return Json(new { success = false, message = "Order ID required" });

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

                await transaction.CommitAsync();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return Json(new { success = false, message = "Transaction failed: " + ex.Message });
            }
        }
        [HttpPost]
        public async Task<JsonResult> SendDetail(int detailId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var detail = await _context.OrderDetails
                    .Include(d => d.Product)
                    .Include(d => d.Order)
                    .ThenInclude(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                    .FirstOrDefaultAsync(d => d.Id == detailId);

                if (detail == null)
                    return Json(new { success = false, message = "Order detail not found." });

                var order = detail.Order;

                if (order.Status != OrderStatus.Pending && order.Status != OrderStatus.Processing)
                    return Json(new { success = false, message = "Order cannot be partially delivered." });

                if (detail.IsShipped)
                    return Json(new { success = false, message = "This detail has already been shipped." });

                if (detail.Product.StockQuantity < detail.Quantity)
                    return Json(new { success = false, message = $"Not enough stock for {detail.Product.Name}." });

                detail.IsShipped = true;
                _context.OrderDetails.Update(detail);
                detail.Product.StockQuantity -= detail.Quantity;
                _context.Products.Update(detail.Product);

                bool allShipped = order.OrderDetails.All(od => od.IsShipped);
                if (allShipped)
                {
                    order.Status = OrderStatus.Shipped;
                }
                else
                {
                    order.Status = OrderStatus.Processing;
                }
                _context.Orders.Update(order);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<JsonResult> SendOrder(int orderId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var order = await _context.Orders
                    .Include(o => o.OrderDetails)
                    .ThenInclude(d => d.Product)
                    .FirstOrDefaultAsync(o => o.Id == orderId);

                if (order == null)
                    return Json(new { success = false, message = "Order not found." });

                if (order.Status != OrderStatus.Pending)
                    return Json(new { success = false, message = "Order can only be sent in full if pending." });

                if (order.OrderDetails.Any(d => d.IsShipped))
                    return Json(new { success = false, message = "Some items already shipped." });

                bool allEnough = true;
                string insufficientProduct = "";
                foreach (var d in order.OrderDetails)
                {
                    if (d.Product.StockQuantity < d.Quantity)
                    {
                        allEnough = false;
                        insufficientProduct = d.Product.Name;
                        break;
                    }
                }

                if (!allEnough)
                    return Json(new { success = false, message = $"Not enough stock for {insufficientProduct}." });

                foreach (var d in order.OrderDetails)
                {
                    d.IsShipped = true;
                    _context.OrderDetails.Update(d);
                    d.Product.StockQuantity -= d.Quantity;
                    _context.Products.Update(d.Product);
                }

                order.Status = OrderStatus.Shipped;
                _context.Orders.Update(order);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }


        [HttpPost]
        public async Task<JsonResult> Delete(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var od = await _context.OrderDetails.FindAsync(id);
                if (od == null)
                    return Json(new { success = false, message = "Not found" });

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

                await transaction.CommitAsync();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return Json(new { success = false, message = "Transaction failed: " + ex.Message });
            }
        }
    }
}
