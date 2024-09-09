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
        private readonly UserService _userService;
        private readonly ProductService _productService;
        private readonly AddressService _addressService;

        public OrderService(AddressService addressService, ProductService productService, UserService userService, OrderRepository orderRepository, ShoppingCartService shoppingCartService, IOrderProcessingService orderProcessingService)
        {
            _orderRepository = orderRepository;
            _shoppingCartService = shoppingCartService;
            _orderProcessingService = orderProcessingService;
            _userService = userService;
            _productService = productService;
            _addressService = addressService;
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

        public async Task DeleteOrderAsync(ObjectId id)
        {
            await _orderRepository.DeleteOrderAsync(id);
        }

        public async Task UpdateOrderAsync(ObjectId id, Order updatedOrder)
        {
            await _orderRepository.UpdateOrderAsync(id, updatedOrder);
        }

        public async Task PlaceOrderAsync(ObjectId userId)
        {
            await _orderRepository.PlaceOrderAsync(userId);
        }

        public async Task CreateOrderAsync(ObjectId userId, List<CartItem> items)
        {
            await _orderRepository.CreateOrderAsync(userId, items);
        }
    }
}
