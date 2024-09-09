    using SiparisUygulamasi.Models;
    using MongoDB.Bson;
    using SiparisUygulamasi.Repositories;
    using SiparisUygulamasi.Services.OrderServices;

    namespace SiparisUygulamasi.Services
    {
        public class ShoppingCartService
        {
            private readonly ShoppingCartRepository _shoppingCartRepository;
            private readonly ProductService _productService;
            private readonly IOrderProcessingService _orderProcessingService;

            public ShoppingCartService(ShoppingCartRepository shoppingCartRepository, ProductService productService, IOrderProcessingService orderProcessingService)
            {
                _shoppingCartRepository = shoppingCartRepository;
                _productService = productService;
                _orderProcessingService = orderProcessingService;
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
            await _orderProcessingService.ProcessOrderAsync(userId);
        }
    }
}