    using SiparisUygulamasi.Models;
    using MongoDB.Bson;
    using SiparisUygulamasi.Repositories;
    using SiparisUygulamasi.Services.OrderServices;

    namespace SiparisUygulamasi.Services
    {
        public class ShoppingCartService
        {
            private readonly ShoppingCartRepository _shoppingCartRepository;
            private readonly ProductRepository _productRepository;
            private readonly IOrderProcessingService _orderProcessingService;

            public ShoppingCartService(ShoppingCartRepository shoppingCartRepository, ProductRepository productRepository, IOrderProcessingService orderProcessingService)
            {
                _shoppingCartRepository = shoppingCartRepository;
                _productRepository = productRepository;
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
            var cart = await _shoppingCartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                cart = await _shoppingCartRepository.CreateNewCartAsync(userId); // Create a new cart without setting Id
            }

            var product = await _productRepository.GetProductByIdAsync(productId);
            if (product == null)
            {
                throw new Exception("Product not found.");
            }

            var existingItem = cart.Items.FirstOrDefault(item => item.ProductId == productId);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity; // Update quantity if the item already exists in the cart
            }
            else
            {
                var newItem = new CartItem { ProductId = productId, Product = product.Name, Price = product.Price, Quantity = quantity };
                cart.Items.Add(newItem);
            }
            await _shoppingCartRepository.UpdateCartAsync(cart);
        }

        public async Task RemoveItemFromCartAsync(ObjectId userId, ObjectId productId)
        {
            var cart = await _shoppingCartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                throw new Exception("Cart not found.");
            }

            var itemToRemove = cart.Items.FirstOrDefault(item => item.ProductId == productId);
            if (itemToRemove != null)
            {
                cart.Items.Remove(itemToRemove);
                await _shoppingCartRepository.UpdateCartAsync(cart);
            }
        }

        public async Task ClearCartAsync(ObjectId userId)
        {
            var cart = await _shoppingCartRepository.GetCartByUserIdAsync(userId);
            if (cart != null)
            {
                cart.Items.Clear();
                await _shoppingCartRepository.UpdateCartAsync(cart);
            }
        }

        public async Task TransferCartToOrderAsync(ObjectId userId)
        {
            await _orderProcessingService.ProcessOrderAsync(userId);
        }
    }
}