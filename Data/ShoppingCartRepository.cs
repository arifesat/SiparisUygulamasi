using MongoDB.Bson;
using MongoDB.Driver;
using SiparisUygulamasi.Data;
using SiparisUygulamasi.Models;

namespace SiparisUygulamasi.Data
{
    public class ShoppingCartRepository
    {
        private readonly IMongoCollection<ShoppingCart> _shoppingCarts;

        public ShoppingCartRepository(MongoDBContext context)
        {
            _shoppingCarts = context.ShoppingCarts;
        }

        public async Task<ShoppingCart> GetCartByUserIdAsync(ObjectId userId)
        {
            return await _shoppingCarts.Find(cart => cart.UserId == userId.ToString()).FirstOrDefaultAsync();
        }

        public async Task AddCartAsync(ShoppingCart cart)
        {
            await _shoppingCarts.InsertOneAsync(cart);
        }

        public async Task UpdateCartAsync(ShoppingCart updatedCart)
        {
            await _shoppingCarts.ReplaceOneAsync(cart => cart.UserId == updatedCart.UserId, updatedCart, new ReplaceOptions { IsUpsert = true });
        }

        public async Task DeleteCartAsync(ObjectId userId)
        {
            await _shoppingCarts.DeleteOneAsync(cart => cart.UserId == userId.ToString());
        }
    }
}

