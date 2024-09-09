using Microsoft.AspNetCore.Mvc;
using SiparisUygulamasi.Data;
using SiparisUygulamasi.Models;
using SiparisUygulamasi.Models.Request;
using SiparisUygulamasi.Services.OrderServices;
using MongoDB.Driver;
using MongoDB.Bson;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly ILogger<OrderController> _logger;
    private readonly OrderService _orderService;

    public OrderController(ILogger<OrderController> logger, OrderService orderService)
    {
        _logger = logger;
        _orderService = orderService;
    }

    [HttpGet]
    public async Task<IEnumerable<Order>> Get()
    {
        try
        {
            return await _orderService.GetAllOrdersAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving orders.");
            throw;
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Order>> Get(string id)
    {
        if (!ObjectId.TryParse(id, out var objectId))
        {
            return BadRequest("Invalid ID format.");
        }
        
        var order = await _orderService.GetOrderByIdAsync(objectId);

        if (order == null)
        {
            return NotFound();
        }

        return Ok(order);
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<Order>>> GetByUserId(string userId)
    {
        if (!ObjectId.TryParse(userId, out var userObjectId))
        {
            return BadRequest("Invalid UserId format.");
        }

        var orders = await _orderService.GetOrdersByUserIdAsync(userObjectId);

        if (orders == null || !orders.Any())
        {
            return NotFound();
        }

        return orders;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
    {
        if (!ObjectId.TryParse(request.UserId, out var userId))
        {
            return BadRequest("Invalid user ID format.");
        }

        var cartItems = request.Items.Select(item => new CartItem
        {
            ProductId = ObjectId.Parse(item.ProductId),
            Quantity = item.Quantity
        }).ToList();

        try
        {
            await _orderService.CreateOrderAsync(userId, cartItems);
            return Ok("Order created successfully.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, Order orderIn)
    {
        if (!ObjectId.TryParse(id, out var objectId))
        {
            return BadRequest("Invalid ID format.");
        }

        var order = _orderService.GetOrderByIdAsync(objectId);

        if (order == null)
        {
            return NotFound();
        }

        await _orderService.UpdateOrderAsync(objectId, orderIn);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        if (!ObjectId.TryParse(id, out var objectId))
        {
            return BadRequest("Invalid ID format.");
        }

        var order = await _orderService.GetOrderByIdAsync(objectId);

        if (order == null)
        {
            return NotFound();
        }

        await _orderService.DeleteOrderAsync(objectId);

        return NoContent();
    }
}