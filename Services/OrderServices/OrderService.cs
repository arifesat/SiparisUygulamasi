using MongoDB.Bson;
using SiparisUygulamasi.Models;
using SiparisUygulamasi.Repositories;

namespace SiparisUygulamasi.Services.OrderServices
{
    public class OrderService : IOrderService
    {
        private readonly Lazy<IOrderRepository> _orderRepository;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IUserService _userService;
        private readonly IProductService _productService;
        private readonly IAddressService _addressService;

        public OrderService(IAddressService addressService, IProductService productService, IUserService userService, Lazy<IOrderRepository> orderRepository, IShoppingCartService shoppingCartService)
        {
            _orderRepository = orderRepository;
            _shoppingCartService = shoppingCartService;
            _userService = userService;
            _productService = productService;
            _addressService = addressService;
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _orderRepository.Value.GetOrdersAsync();
        }

        public async Task<Order> GetOrderByIdAsync(ObjectId id)
        {
            return await _orderRepository.Value.GetOrderByIdAsync(id);
        }

        public async Task<List<Order>> GetOrdersByUserIdAsync(ObjectId userId)
        {
            return await _orderRepository.Value.GetOrdersByUserIdAsync(userId);
        }

        public async Task DeleteOrderAsync(ObjectId id)
        {
            await _orderRepository.Value.DeleteOrderAsync(id);
        }

        public async Task UpdateOrderAsync(ObjectId id, Order updatedOrder)
        {
            await _orderRepository.Value.UpdateOrderAsync(id, updatedOrder);
        }

        public async Task PlaceOrderAsync(ObjectId userId)
        {
            await _orderRepository.Value.PlaceOrderAsync(userId);
        }

        public async Task CreateOrderAsync(ObjectId userId, List<CartItem> items)
        {
            await _orderRepository.Value.CreateOrderAsync(userId, items);
        }

        public async Task AddOrderAsync(Order order)
        {
            await _orderRepository.Value.AddOrderAsync(order);
        }
    }
}
