using Microsoft.AspNetCore.Mvc;
using SiparisUygulamasi.Data;
using SiparisUygulamasi.Models;
using SiparisUygulamasi.Models.Request;
using SiparisUygulamasi.Services.OrderServices;
using SiparisUygulamasi.Models.Request.UserRequest;
using MongoDB.Driver;
using MongoDB.Bson;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly MongoDBContext _context;
    private readonly ILogger<OrderController> _logger;
    private readonly OrderService _orderService;

    public OrderController(MongoDBContext context, ILogger<OrderController> logger, OrderService orderService)
    {
        _context = context;
        _logger = logger;
        _orderService = orderService;
    }
    //This code block returns all of the orders
    //[HttpGet]
    //public async Task<IEnumerable<Order>> Get()
    //{
    //    //return await _context.Orders.Find(_ => true).ToListAsync();

    //    try
    //    {
    //        return await _context.Orders.Find(_ => true).ToListAsync();
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError(ex, "An error occurred while retrieving orders.");
    //        throw;
    //    }
    //}

    [HttpGet("{id}")]
    public async Task<ActionResult<Order>> Get(string id)
    {
        if (!ObjectId.TryParse(id, out var objectId))
        {
            return BadRequest("Invalid ID format.");
        }

        var order = await _context.Orders.Find(p => p.Id == objectId).FirstOrDefaultAsync();

        if (order == null)
        {
            return NotFound();
        }

        return order;
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<Order>>> GetByUserId(string userId)
    {
        if (!ObjectId.TryParse(userId, out var userObjectId))
        {
            return BadRequest("Invalid UserId format.");
        }

        var orders = await _context.Orders.Find(p => p.UserId == userObjectId).ToListAsync();

        if (orders == null || !orders.Any())
        {
            return NotFound();
        }

        return orders;
    }

    //[HttpPost]
    //public async Task<ActionResult<Order>> Create(Order order)
    //{
    //    await _context.Orders.InsertOneAsync(order);
    //    return CreatedAtRoute(new { id = order.Id }, order);
    //}

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

        var order = await _context.Orders.Find(p => p.Id == objectId).FirstOrDefaultAsync();

        if (order == null)
        {
            return NotFound();
        }

        await _context.Orders.ReplaceOneAsync(p => p.Id == objectId, orderIn);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        if (!ObjectId.TryParse(id, out var objectId))
        {
            return BadRequest("Invalid ID format.");
        }

        var order = await _context.Orders.Find(p => p.Id == objectId).FirstOrDefaultAsync();

        if (order == null)
        {
            return NotFound();
        }

        await _context.Orders.DeleteOneAsync(p => p.Id == objectId);

        return NoContent();
    }
}