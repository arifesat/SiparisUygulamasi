using SiparisUygulamasi.Data;
using SiparisUygulamasi.Models;
using MongoDB.Bson;

namespace SiparisUygulamasi.Services
{
    public class ShoppingCartService
    {
        private readonly ShoppingCartRepository _shoppingCartRepository;
        private readonly ProductRepository _productRepository;

        public ShoppingCartService(ShoppingCartRepository shoppingCartRepository, ProductRepository productRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _productRepository = productRepository;
        }

        public async Task<ShoppingCart> CreateNewCartAsync()
        {
            return await _shoppingCartRepository.CreateNewCartAsync();
        }

        public async Task<ShoppingCart> GetCartByUserIdAsync(ObjectId userId)
        {
            return await _shoppingCartRepository.GetCartByUserIdAsync(userId);
        }

        public async Task AddItemToCartAsync(ObjectId userId, ObjectId productId, int quantity)
        {
            //var cart = await _shoppingCartRepository.GetCartByUserIdAsync(userId) ?? new ShoppingCart { UserId = userId, Items = new List<CartItem>() };
            var cart = await _shoppingCartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                cart = new ShoppingCart { UserId = userId }; // Create a new cart without setting Id
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
                cart.Items.Add(new CartItem { ProductId = productId, Product = product.Name, Price = product.Price, Quantity = quantity });
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
    }
}
