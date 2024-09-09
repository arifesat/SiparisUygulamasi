using MongoDB.Bson;
using SiparisUygulamasi.Models;
using SiparisUygulamasi.Repositories;
using SiparisUygulamasi.Services;
using SiparisUygulamasi.Services.OrderServices;


public class OrderProcessingService : IOrderProcessingService
{
    private readonly Func<IOrderService> _orderServiceFactory;
    private readonly IShoppingCartService _shoppingCartService;
    private readonly IOrderProcessingHelper _orderProcessingHelper;

    public OrderProcessingService(Func<IOrderService> orderServiceFactory, IShoppingCartService shoppingCartService)
    {
        _orderServiceFactory = orderServiceFactory;
        _shoppingCartService = shoppingCartService;
    }

    public async Task ProcessOrderAsync(ObjectId userId)
    {

        await _orderProcessingHelper.ProcessOrderAsync(userId);

        //var orderService = _orderServiceFactory();
        //var cart = await _shoppingCartService.GetCartByUserIdAsync(userId);

        //if (cart == null || !cart.Items.Any())
        //{
        //    throw new Exception("Cart is empty, cannot place order.");
        //}

        //var orderItems = cart.Items.Select(item => new CartItem
        //{
        //    ProductId = item.ProductId,
        //    Product = item.Product,
        //    Quantity = item.Quantity,
        //    Price = item.Price
        //}).ToList();

        //var order = new Order
        //{
        //    UserId = userId,
        //    Items = orderItems,
        //    TotalAmount = orderItems.Sum(item => item.Quantity * item.Price),
        //    OrderDate = DateTime.UtcNow
        //};

        //await orderService.AddOrderAsync(order);
        //await _shoppingCartService.ClearCartAsync(userId);
    }
}
