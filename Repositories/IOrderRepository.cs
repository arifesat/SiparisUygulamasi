using MongoDB.Bson;
using SiparisUygulamasi.Models;

namespace SiparisUygulamasi.Repositories
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetOrdersAsync();
        Task<Order> GetOrderByIdAsync(ObjectId id);
        Task<List<Order>> GetOrdersByUserIdAsync(ObjectId userId);
        Task DeleteOrderAsync(ObjectId id);
        Task UpdateOrderAsync(ObjectId id, Order updatedOrder);
        Task PlaceOrderAsync(ObjectId userId);
        Task CreateOrderAsync(ObjectId userId, List<CartItem> items);
        Task AddOrderAsync(Order order);
    }
}
