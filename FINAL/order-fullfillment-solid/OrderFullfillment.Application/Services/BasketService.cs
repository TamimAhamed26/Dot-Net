using System.Linq;
using System.Threading.Tasks;
using OrderFullfillment.Application.SeedWorks;
using OrderFullfillment.Application.Services.Interfaces;
using OrderFullfillment.Entity.Entities;
using OrderFullfillment.Entity.Entities.Basket;
using OrderFullfillment.Infrastructure.SeedWorks;

namespace OrderFullfillment.Application.Services
{
    public class BasketService : BaseService, IBasketService
    {
        private readonly IRepository<Basket> _basketRepo;
        private readonly IRepository<Product> _productRepo;

        public BasketService(IUnitOfWork unitOfWork, IRepository<Basket> basketRepo, IRepository<Product> productRepo) : base(unitOfWork)
        {
            _basketRepo = basketRepo;
            _productRepo = productRepo;
        }

        public async Task<Basket> Get(int id)
        {
            return await _basketRepo.GetAsync(id);
        }

        public async Task<Basket> Create(int userId)
        {
            var basket = new Basket(userId);
            _basketRepo.Add(basket);
            await UnitOfWork.SaveChangeAsync();
            return basket;
        }

        public async Task AddItem(int basketId, int productId)
        {
            var basket = await _basketRepo.GetAsync(basketId);
            var productItem = basket.Products.FirstOrDefault(_ => _.Product.Id == productId);
            if (productItem == null)
            {
                var product = await _productRepo.GetAsync(productId);
                productItem = new BasketProductItem(product);
                basket.Products.Add(productItem);
            }
            else
            {
                productItem.Quantity += 1;
            }

            await UnitOfWork.SaveChangeAsync();
        }

        public async Task RemoveItem(int basketId, int itemId)
        {
            var basket = await _basketRepo.GetAsync(basketId);
            var productItem = basket.Products.FirstOrDefault(_ => _.Id == itemId);
            if (productItem != null)
            {
                basket.Products.Remove(productItem);
                await UnitOfWork.SaveChangeAsync();
            }
        }

        public async Task MarkAsResolved(int basketId)
        {
            var basket = await _basketRepo.GetAsync(basketId);
            basket.IsCheckedOut = true;
            await UnitOfWork.SaveChangeAsync();
        }
    }
}