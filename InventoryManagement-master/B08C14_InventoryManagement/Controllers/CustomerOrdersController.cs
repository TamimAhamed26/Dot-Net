using B08C14_InventoryManagement.Data;
using B08C14_InventoryManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace B08C14_InventoryManagement.Controllers
{
    [Authorize(Roles = "Customer")]
    public class CustomerOrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CustomerOrdersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.UserId == user.Id);
            if (customer == null) return Unauthorized();

            var orders = await _context.Orders
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .Where(o => o.CustomerId == customer.Id)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return View(orders);
        }

        [HttpPost]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var customer = await _context.Customers.FirstOrDefaultAsync(c => c.UserId == user.Id);

                var order = await _context.Orders
                    .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                    .FirstOrDefaultAsync(o => o.Id == orderId && o.CustomerId == customer.Id);

                if (order == null)
                    return Json(new { success = false, message = "Order not found." });

                if (order.Status != OrderStatus.Shipped)
                    return Json(new { success = false, message = "Only shipped orders can be cancelled." });

                foreach (var detail in order.OrderDetails)
                {
                    if (detail.Product != null)
                    {
                        detail.Product.StockQuantity += detail.Quantity;
                        _context.Products.Update(detail.Product);
                    }
                }

                order.Status = OrderStatus.Cancelled;
                _context.Update(order);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return Json(new { success = true, message = "Order cancelled and stock restored successfully." });
            }
            catch
            {
                await transaction.RollbackAsync();
                return Json(new { success = false, message = "An error occurred while cancelling the order." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AcceptOrder(int orderId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var customer = await _context.Customers.FirstOrDefaultAsync(c => c.UserId == user.Id);

                var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId && o.CustomerId == customer.Id);

                if (order == null)
                    return Json(new { success = false, message = "Order not found." });

                if (order.Status != OrderStatus.Shipped)
                    return Json(new { success = false, message = "Only shipped orders can be accepted." });

                order.Status = OrderStatus.Delivered;
                _context.Update(order);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return Json(new { success = true, message = "Order marked as delivered." });
            }
            catch
            {
                await transaction.RollbackAsync();
                return Json(new { success = false, message = "An error occurred while accepting the order." });
            }
        }
    }
}
