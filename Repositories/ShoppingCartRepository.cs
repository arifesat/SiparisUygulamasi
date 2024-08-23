using MongoDB.Bson;
using MongoDB.Driver;
using SiparisUygulamasi.Data;
using SiparisUygulamasi.Models;

namespace SiparisUygulamasi.Repositories
{
    public class ShoppingCartRepository
    {
        private readonly IMongoCollection<ShoppingCart> _shoppingCarts;

        public ShoppingCartRepository(MongoDBContext context)
        {
            _shoppingCarts = context.ShoppingCarts;
        }

        public async Task<ShoppingCart> CreateNewCartAsync(ObjectId userId)
        {
            var newCart = new ShoppingCart
            {
                Id = userId,
                UserId = userId,
                Items = new List<CartItem>()
            };

            await _shoppingCarts.InsertOneAsync(newCart);
            return newCart;
        }

        public async Task<ShoppingCart> GetCartByUserIdAsync(ObjectId userId)
        {
            return await _shoppingCarts.Find(cart => cart.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task AddItemToCartAsync(ObjectId userId, CartItem item)
        {
            var filter = Builders<ShoppingCart>.Filter.Eq(cart => cart.UserId, userId);
            var update = Builders<ShoppingCart>.Update.Push(cart => cart.Items, item);
            await _shoppingCarts.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });
        }

        public async Task UpdateCartAsync(ShoppingCart updatedCart)
        {
            await _shoppingCarts.ReplaceOneAsync(cart => cart.UserId == updatedCart.UserId, updatedCart, new ReplaceOptions { IsUpsert = true });
        }

        public async Task DeleteCartAsync(ObjectId userId)
        {
            await _shoppingCarts.DeleteOneAsync(cart => cart.UserId == userId);
        }
    }
}

