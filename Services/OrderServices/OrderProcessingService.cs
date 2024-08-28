using MongoDB.Bson;
using SiparisUygulamasi.Models;
using SiparisUygulamasi.Repositories;

public class OrderProcessingService : IOrderProcessingService
{
    private readonly OrderRepository _orderRepository;
    private readonly ShoppingCartRepository _shoppingCartRepository;

    public OrderProcessingService(OrderRepository orderRepository, ShoppingCartRepository shoppingCartRepository)
    {
        _orderRepository = orderRepository;
        _shoppingCartRepository = shoppingCartRepository;
    }

    public async Task ProcessOrderAsync(ObjectId userId)
    {
        // Get the shopping cart for the user
        var cart = await _shoppingCartRepository.GetCartByUserIdAsync(userId);

        if (cart == null || !cart.Items.Any())
        {
            throw new Exception("Cart is empty, cannot place order.");
        }

        // Create the order based on the shopping cart items
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

        await _orderRepository.AddOrderAsync(order);

        // Clear the shopping cart after placing the order
        await _shoppingCartRepository.ClearCartAsync(userId);
    }
}