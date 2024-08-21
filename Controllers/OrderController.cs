using Microsoft.AspNetCore.Mvc;
using SiparisUygulamasi.Data;
using MongoDB.Driver;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly MongoDBContext _context;
    private readonly ILogger<OrderController> _logger;


    public OrderController(MongoDBContext context, ILogger<OrderController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IEnumerable<Order>> Get()
    {
        //return await _context.Orders.Find(_ => true).ToListAsync();

        try
        {
            return await _context.Orders.Find(_ => true).ToListAsync();
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
        var order = await _context.Orders.Find(p => p.Id == id).FirstOrDefaultAsync();

        if (order == null)
        {
            return NotFound();
        }

        return order;
    }

    [HttpPost]
    public async Task<ActionResult<Order>> Create(Order order)
    {
        await _context.Orders.InsertOneAsync(order);
        return CreatedAtRoute(new { id = order.Id }, order);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, Order orderIn)
    {
        var order = await _context.Orders.Find(p => p.Id == id).FirstOrDefaultAsync();

        if (order == null)
        {
            return NotFound();
        }

        await _context.Orders.ReplaceOneAsync(p => p.Id == id, orderIn);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var order = await _context.Orders.Find(p => p.Id == id).FirstOrDefaultAsync();

        if (order == null)
        {
            return NotFound();
        }

        await _context.Orders.DeleteOneAsync(p => p.Id == id);

        return NoContent();
    }
}