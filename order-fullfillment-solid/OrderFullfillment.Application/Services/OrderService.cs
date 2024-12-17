using System.Threading.Tasks;
using OrderFullfillment.Application.SeedWorks;
using OrderFullfillment.Application.Services.Interfaces;
using OrderFullfillment.Application.ViewModels.Order;
using OrderFullfillment.Entity;
using OrderFullfillment.Entity.Entities.Basket;
using OrderFullfillment.Entity.Entities.Orders;
using OrderFullfillment.Infrastructure.SeedWorks;

namespace OrderFullfillment.Application.Services
{
    public class OrderService : BaseService, IOrderService
    {
        private readonly IBasketService _basketService;
        private readonly IInvoiceService _invoiceService;

        private readonly IRepository<Order> _orderRepo;
        private readonly IRepository<Basket> _basketRepo;

        public OrderService(IUnitOfWork unitOfWork, 
            IBasketService basketService, 
            IRepository<Order> orderRepo,
            IRepository<Basket> basketRepo,
            IInvoiceService invoiceService) : base(unitOfWork)
        {
            _orderRepo = orderRepo;
            _basketRepo = basketRepo;
            _basketService = basketService;
            _invoiceService = invoiceService;
        }

        public async Task<Order> Get(int id)
        {
            return await _orderRepo.GetAsync(id);
        }

        public async Task<Order> CreateOrder(OrderRequestVM orderInfo)
        {
            var order = new Order(orderInfo.Type, orderInfo.CustomerName, orderInfo.Address);
            var basket = await _basketRepo.GetAsync(orderInfo.BasketId);
            foreach (var item in basket.Products)
            {
                order.AddProductItem(item.Product, item.Quantity);
            }

            await UnitOfWork.ExecuteTransactionAsync(async () =>
            {
                _orderRepo.Add(order);
                await _basketService.MarkAsResolved(orderInfo.BasketId);
                return await UnitOfWork.SaveChangeAsync();
            });
            return order;
        }

        public async Task MarkOrderAsPaid(int orderId)
        {
            var order = await _orderRepo.GetAsync(orderId);
            order.Status = OrderStatus.Paid;
            await UnitOfWork.ExecuteTransactionAsync(async () =>
            {
                order.InvoiceId = await _invoiceService.CreateInvoice(order);
                return await UnitOfWork.SaveChangeAsync();
            });
        }
    }
}