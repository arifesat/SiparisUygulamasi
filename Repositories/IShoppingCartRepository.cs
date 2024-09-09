using MongoDB.Bson;

namespace SiparisUygulamasi.Repositories
{
    public interface IShoppingCartRepository
    {
        Task<ShoppingCart> CreateNewCartAsync(ObjectId userId);
        Task<ShoppingCart> GetCartByUserIdAsync(ObjectId userId);
        Task AddItemToCartAsync(ObjectId userId, ObjectId productId, int quantity);
        Task RemoveItemFromCartAsync(ObjectId userId, ObjectId productId);
        Task ClearCartAsync(ObjectId userId);
        Task UpdateCartAsync(ShoppingCart updatedCart);
        Task DeleteCartAsync(ObjectId userId);
    }
}
