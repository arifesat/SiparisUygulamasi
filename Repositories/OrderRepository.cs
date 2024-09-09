using MongoDB.Bson;
using MongoDB.Driver;
using SiparisUygulamasi.Data;
using SiparisUygulamasi.Models;
using SiparisUygulamasi.Services;

namespace SiparisUygulamasi.Repositories
{
    public class OrderRepository
    {
        private readonly IMongoCollection<Order> _orders;
        private readonly ShoppingCartService _shoppingCartService;
        private readonly OrderRepository _orderRepository;
        private readonly UserRepository _userRepository;
        private readonly ProductRepository _productRepository;
        private readonly AddressService _addressService;


        public OrderRepository(MongoDBContext context, ShoppingCartService shoppingCartService, OrderRepository orderRepository, UserRepository userRepository
            , ProductRepository productRepository, AddressService addressService)
        {
            _orders = context.Orders;
            _shoppingCartService = shoppingCartService;
            _orderRepository = orderRepository;
            _addressService = addressService;
            _userRepository = userRepository;
            _productRepository = productRepository;
        }

        public async Task<List<Order>> GetOrdersAsync()
        {
            return await _orders.Find(order => true).ToListAsync();
        }

        public async Task<Order> GetOrderByIdAsync(ObjectId id)
        {
            return await _orders.Find(order => order.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Order>> GetOrdersByUserIdAsync(ObjectId userId)
        {
            return await _orders.Find(order => order.UserId == userId).ToListAsync();
        }

        public async Task AddOrderAsync(Order order)
        {
            await _orders.InsertOneAsync(order);
        }

        public async Task UpdateOrderAsync(ObjectId id, Order updatedOrder)
        {
            await _orders.ReplaceOneAsync(order => order.Id == id, updatedOrder);
        }

        public async Task DeleteOrderAsync(ObjectId id)
        {
            await _orders.DeleteOneAsync(order => order.Id == id);
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

            var address = await _addressService.GetAddressesByUserIdAsync(userId);
            if (address == null || !address.Any())
            {
                throw new Exception("User address not found.");
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
                Address = address.First() // Assuming the user has an Address property
            };

            await _orderRepository.AddOrderAsync(order);
        }
    }
}
