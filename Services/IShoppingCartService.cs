using MongoDB.Bson;

namespace SiparisUygulamasi.Services
{
    public interface IShoppingCartService
    {
        Task<ShoppingCart> CreateNewCartAsync(ObjectId userId);
        Task<ShoppingCart> GetCartByUserIdAsync(ObjectId userId);
        Task AddItemToCartAsync(ObjectId userId, ObjectId productId, int quantity);
        Task RemoveItemFromCartAsync(ObjectId userId, ObjectId productId);
        Task ClearCartAsync(ObjectId userId);
        Task TransferCartToOrderAsync(ObjectId userId);
    }
}
