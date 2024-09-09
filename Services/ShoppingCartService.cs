using SiparisUygulamasi.Models;
using MongoDB.Bson;
using SiparisUygulamasi.Repositories;

namespace SiparisUygulamasi.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IProductService _productService;
        private readonly Func<IOrderProcessingService> _orderProcessingServiceFactory;

        public ShoppingCartService(IShoppingCartRepository shoppingCartRepository, IProductService productService, Func<IOrderProcessingService> orderProcessingServiceFactory)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _productService = productService;
            _orderProcessingServiceFactory = orderProcessingServiceFactory;
        }

        public async Task<ShoppingCart> CreateNewCartAsync(ObjectId userId)
        {
            return await _shoppingCartRepository.CreateNewCartAsync(userId);
        }

        public async Task<ShoppingCart> GetCartByUserIdAsync(ObjectId userId)
        {
            return await _shoppingCartRepository.GetCartByUserIdAsync(userId);
        }

        public async Task AddItemToCartAsync(ObjectId userId, ObjectId productId, int quantity)
        {
            await _shoppingCartRepository.AddItemToCartAsync(userId, productId, quantity);
        }

        public async Task RemoveItemFromCartAsync(ObjectId userId, ObjectId productId)
        {
            await _shoppingCartRepository.RemoveItemFromCartAsync(userId, productId);
        }

        public async Task ClearCartAsync(ObjectId userId)
        {
            await _shoppingCartRepository.ClearCartAsync(userId);
        }

        public async Task TransferCartToOrderAsync(ObjectId userId)
        {
            var orderProcessingService = _orderProcessingServiceFactory();
            await orderProcessingService.ProcessOrderAsync(userId);
        }
    }
}