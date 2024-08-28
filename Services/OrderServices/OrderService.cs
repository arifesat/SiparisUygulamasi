using MongoDB.Bson;
using SiparisUygulamasi.Models;
using SiparisUygulamasi.Repositories;

namespace SiparisUygulamasi.Services.OrderServices
{
    public class OrderService
    {
        private readonly OrderRepository _orderRepository;
        private readonly ShoppingCartService _shoppingCartService;
        private readonly IOrderProcessingService _orderProcessingService;
        private readonly UserRepository _userRepository;
        private readonly ProductRepository _productRepository;

        public OrderService(ProductRepository productRepository, UserRepository userRepository, OrderRepository orderRepository, ShoppingCartService shoppingCartService, IOrderProcessingService orderProcessingService)
        {
            _orderRepository = orderRepository;
            _shoppingCartService = shoppingCartService;
            _orderProcessingService = orderProcessingService;
            _userRepository = userRepository;
            _productRepository = productRepository;
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
            var orderItems = cart.Items.Select(item => new CartItem
            {
                ProductId = item.ProductId, // Convert ObjectId to string
                Product = item.Product,
                Quantity = item.Quantity,
                Price = item.Price
            }).ToList();

            var order = new Order
            {
                UserId = userId,
                Items = orderItems,
                TotalAmount = orderItems.Sum(item => item.Quantity * item.Price),
                OrderDate = DateTime.UtcNow
            };

            await _orderRepository.AddOrderAsync(order);

            // Clear the shopping cart after placing the order
            await _shoppingCartService.ClearCartAsync(userId);
        }

        public async Task CreateOrderAsync(ObjectId userId, List<CartItem> items)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            var orderItems = new List<CartItem>();
            foreach (var item in items)
            {
                var productId = item.ProductId;

                var product = await _productRepository.GetProductByIdAsync(productId);
                if (product == null)
                {
                    throw new Exception($"Product with ID {item.ProductId} not found.");
                }

                orderItems.Add(new CartItem
                {
                    ProductId = productId,
                    Product = product.Name,
                    Quantity = item.Quantity,
                    Price = product.Price
                });
            }

            var order = new Order
            {
                UserId = userId,
                Items = orderItems,
                TotalAmount = orderItems.Sum(i => i.Quantity * i.Price),
                OrderDate = DateTime.UtcNow,
                Address = user.Address // Assuming the user has an Address property
            };

            await _orderRepository.AddOrderAsync(order);
        }
    }
}
