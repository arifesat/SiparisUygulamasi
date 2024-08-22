using MongoDB.Bson;
using SiparisUygulamasi.Data;
using SiparisUygulamasi.Models;

namespace SiparisUygulamasi.Services
{
    public class OrderService
    {
        private readonly OrderRepository _orderRepository;
        private readonly ShoppingCartService _shoppingCartService;

        public OrderService(OrderRepository orderRepository, ShoppingCartService shoppingCartService)
        {
            _orderRepository = orderRepository;
            _shoppingCartService = shoppingCartService;
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _orderRepository.GetOrdersAsync();
        }

        public async Task<Order> GetOrderByIdAsync(ObjectId id)
        {
            return await _orderRepository.GetOrderByIdAsync(id);
        }

        public async Task<List<Order>> GetOrdersByUserIdAsync(ObjectId userId)
        {
            return await _orderRepository.GetOrdersByUserIdAsync(userId);
        }

        public async Task PlaceOrderAsync(ObjectId userId)
        {
            // Get the shopping cart for the user
            var cart = await _shoppingCartService.GetCartByUserIdAsync(userId);

            if (cart == null || !cart.Items.Any())
            {
                throw new Exception("Cart is empty, cannot place order.");
            }

            // Create the order based on the shopping cart items
            var orderItems = cart.Items.Select(item => new OrderItem
            {
                ProductId = item.ProductId.ToString(), // Convert ObjectId to string
                ProductName = item.Product,
                Quantity = item.Quantity,
                Price = item.Price
            }).ToList();

            var order = new Order
            {
                UserId = userId.ToString(),
                Items = orderItems,
                TotalAmount = orderItems.Sum(item => item.Quantity * item.Price),
                Status = "Pending",
                OrderDate = DateTime.UtcNow
            };

            await _orderRepository.AddOrderAsync(order);

            // Clear the shopping cart after placing the order
            await _shoppingCartService.ClearCartAsync(userId);
        }

        public async Task UpdateOrderStatusAsync(ObjectId id, string status)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);
            if (order == null)
            {
                throw new Exception("Order not found.");
            }

            order.Status = status;
            await _orderRepository.UpdateOrderAsync(id, order);
        }
    }
}
