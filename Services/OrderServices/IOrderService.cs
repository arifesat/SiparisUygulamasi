﻿using MongoDB.Bson;
using SiparisUygulamasi.Models;

namespace SiparisUygulamasi.Services.OrderServices
{
    public interface IOrderService
    {
        Task<List<Order>> GetAllOrdersAsync();

        Task<Order> GetOrderByIdAsync(ObjectId id);

        Task<List<Order>> GetOrdersByUserIdAsync(ObjectId userId);

        Task DeleteOrderAsync(ObjectId id);

        Task UpdateOrderAsync(ObjectId id, Order updatedOrder);

        Task PlaceOrderAsync(ObjectId userId);

        Task CreateOrderAsync(ObjectId userId, List<CartItem> items);

        Task AddOrderAsync(Order order);
    }
}
