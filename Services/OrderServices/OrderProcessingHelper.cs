using MongoDB.Bson;
using SiparisUygulamasi.Models;
using System.Threading.Tasks;

namespace SiparisUygulamasi.Services.OrderServices
{
    public class OrderProcessingHelper : IOrderProcessingHelper
    {
        private readonly IOrderService _orderService;
        private readonly IShoppingCartService _shoppingCartService;

        public OrderProcessingHelper(IOrderService orderService, IShoppingCartService shoppingCartService)
        {
            _orderService = orderService;
            _shoppingCartService = shoppingCartService;
        }

        public async Task ProcessOrderAsync(ObjectId userId)
        {
            var cart = await _shoppingCartService.GetCartByUserIdAsync(userId);

            if (cart == null || !cart.Items.Any())
            {
                throw new Exception("Cart is empty, cannot place order.");
            }

            var orderItems = cart.Items.Select(item => new CartItem
            {
                ProductId = item.ProductId,
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

            await _orderService.AddOrderAsync(order);
            await _shoppingCartService.ClearCartAsync(userId);
        }
    }
}