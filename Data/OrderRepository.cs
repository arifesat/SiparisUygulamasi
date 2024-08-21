﻿using SiparisUygulamasi.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace SiparisUygulamasi.Data
{
    public class OrderRepository
    {
        private readonly IMongoCollection<Order> _orders;

        public OrderRepository(MongoDBContext context)
        {
            _orders = context.Orders;
        }

        public async Task<List<Order>> GetOrdersAsync()
        {
            return await _orders.Find(order => true).ToListAsync();
        }

        public async Task<Order> GetOrderByIdAsync(ObjectId id)
        {
            return await _orders.Find(order => order.Id == id.ToString()).FirstOrDefaultAsync();
        }

        public async Task<List<Order>> GetOrdersByUserIdAsync(ObjectId userId)
        {
            return await _orders.Find(order => order.UserId == userId.ToString()).ToListAsync();
        }

        public async Task AddOrderAsync(Order order)
        {
            await _orders.InsertOneAsync(order);
        }

        public async Task UpdateOrderAsync(ObjectId id, Order updatedOrder)
        {
            await _orders.ReplaceOneAsync(order => order.Id == id.ToString(), updatedOrder);
        }

        public async Task DeleteOrderAsync(ObjectId id)
        {
            await _orders.DeleteOneAsync(order => order.Id == id.ToString());
        }
    }
}
