using MongoDB.Bson;
using MongoDB.Driver;
using SiparisUygulamasi.Data;
using SiparisUygulamasi.Models;

namespace SiparisUygulamasi.Repositories
{
    public class ShoppingCartRepository
    {
        private readonly IMongoCollection<ShoppingCart> _shoppingCarts;
        private readonly ProductRepository _productRepository;
        private readonly ShoppingCartRepository _shoppingCartRepository;


        public ShoppingCartRepository(ShoppingCartRepository shoppingCartRepository, MongoDBContext context, ProductRepository productRepository)
        {
            _shoppingCarts = context.ShoppingCarts;
            _productRepository = productRepository;
            _shoppingCartRepository = shoppingCartRepository;
        }

        public async Task<ShoppingCart> CreateNewCartAsync(ObjectId userId)
        {
            var newCart = new ShoppingCart
            {
                Id = userId,
                UserId = userId,
                Items = new List<CartItem>(),
                Status = "Active"
            };

            await _shoppingCarts.InsertOneAsync(newCart);
            return newCart;
        }

        public async Task<ShoppingCart> GetCartByUserIdAsync(ObjectId userId)
        {
            return await _shoppingCarts.Find(cart => cart.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task AddItemToCartAsync(ObjectId userId, ObjectId productId, int quantity)
        {

            var cart = await GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                cart = await CreateNewCartAsync(userId); // Create a new cart without setting Id
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
            await UpdateCartAsync(cart);
        }

        public async Task UpdateCartAsync(ShoppingCart updatedCart)
        {
            await _shoppingCarts.ReplaceOneAsync(cart => cart.UserId == updatedCart.UserId, updatedCart, new ReplaceOptions { IsUpsert = true });
        }

        public async Task DeleteCartAsync(ObjectId userId)
        {
            await _shoppingCarts.DeleteOneAsync(cart => cart.UserId == userId);
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
    }
}

