using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using SiparisUygulamasi.Services;
using SiparisUygulamasi.Data;

[ApiController]
[Route("api/[controller]")]
public class ShoppingCartController : ControllerBase
{
    private readonly ShoppingCartService _shoppingCartService;

    private readonly MongoDBContext _context;

    public ShoppingCartController(ShoppingCartService shoppingCartService, MongoDBContext context)
    {
        _shoppingCartService = shoppingCartService;
        _context = context;
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult<ShoppingCart>> GetCartByUserId(string userId)
    {
        if (!ObjectId.TryParse(userId, out var objectId))
        {
            return BadRequest("Invalid user ID format.");
        }

        var cart = await _shoppingCartService.GetCartByUserIdAsync(objectId);
        if (cart == null)
        {
            return NotFound();
        }

        return cart;
    }

    [HttpPost("{userId}/items")]
    public async Task<IActionResult> AddItemToCart(string userId, [FromBody] AddItemRequest request)
    {
        if (!ObjectId.TryParse(userId, out var objectId))
        {
            return BadRequest("Invalid user ID format.");
        }

        if (!ObjectId.TryParse(request.ProductId, out var productId))
        {
            return BadRequest("Invalid product ID format.");
        }

        var User = await _context.Users.Find(p => p.Id == objectId).FirstOrDefaultAsync();

        await _shoppingCartService.AddItemToCartAsync(objectId, productId, request.Quantity);
        return NoContent();
    }

    [HttpDelete("{userId}/items/{productId}")]
    public async Task<IActionResult> RemoveItemFromCart(string userId, string productId)
    {
        if (!ObjectId.TryParse(userId, out var objectId))
        {
            return BadRequest("Invalid user ID format.");
        }

        if (!ObjectId.TryParse(productId, out var productObjectId))
        {
            return BadRequest("Invalid product ID format.");
        }

        await _shoppingCartService.RemoveItemFromCartAsync(objectId, productObjectId);
        return NoContent();
    }

    [HttpDelete("{userId}")]
    public async Task<IActionResult> ClearCart(string userId)
    {
        if (!ObjectId.TryParse(userId, out var objectId))
        {
            return BadRequest("Invalid user ID format.");
        }

        await _shoppingCartService.ClearCartAsync(objectId);
        return NoContent();
    }
}

public class AddItemRequest
{
    public string ProductId { get; set; }
    public int Quantity { get; set; }
}