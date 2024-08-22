using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using SiparisUygulamasi.Models;
using SiparisUygulamasi.Services;
using System.Threading.Tasks;

namespace SiparisUygulamasi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShoppingCartController : ControllerBase
    {
        private readonly ShoppingCartService _shoppingCartService;

        public ShoppingCartController(ShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
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
}

//using Microsoft.AspNetCore.Mvc;
//using SiparisUygulamasi.Data;
//using SiparisUygulamasi.Models;
//using MongoDB.Driver;
//using MongoDB.Bson;

//[ApiController]
//[Route("api/[controller]")]
//public class ShoppingCartController : ControllerBase
//{
//    private readonly MongoDBContext _context;

//    public ShoppingCartController(MongoDBContext context)
//    {
//        _context = context;
//    }

//    [HttpGet]
//    public async Task<IEnumerable<ShoppingCart>> Get()
//    {
//        return await _context.ShoppingCarts.Find(_ => true).ToListAsync();
//    }

//    [HttpGet("{id}")]
//    public async Task<ActionResult<ShoppingCart>> Get(string id)
//    {
//        var objectId = ObjectId.Parse(id);
//        var ShoppingCart = await _context.ShoppingCarts.Find(p => p.Id == objectId).FirstOrDefaultAsync();

//        if (ShoppingCart == null)
//        {
//            return NotFound();
//        }
//        return ShoppingCart;
//    }

//    [HttpPost]
//    public async Task<ActionResult<ShoppingCart>> Create(ShoppingCart ShoppingCart)
//    {
//        await _context.ShoppingCarts.InsertOneAsync(ShoppingCart);
//        return CreatedAtRoute(new { id = ShoppingCart.Id}, ShoppingCart);
//    }

//    [HttpPut("{id}")]
//    public async Task<IActionResult> Update(string id, ShoppingCart ShoppingCartIn)
//    {
//        var objectId = ObjectId.Parse(id);
//        var ShoppingCart = await _context.ShoppingCarts.Find(p => p.Id == objectId).FirstOrDefaultAsync();

//        if (ShoppingCart == null)
//        {
//            return NotFound();
//        }

//        await _context.ShoppingCarts.ReplaceOneAsync(p => p.Id == objectId, ShoppingCartIn);

//        return NoContent();
//    }

//    [HttpDelete("{id}")]
//    public async Task<IActionResult> Delete(string id)
//    {
//        var objectId = ObjectId.Parse(id);

//        var ShoppingCart = await _context.ShoppingCarts.Find(p => p.Id == objectId).FirstOrDefaultAsync();

//        if (ShoppingCart == null)
//        {
//            return NotFound();
//        }

//        await _context.ShoppingCarts.DeleteOneAsync(p => p.Id == objectId);
//        return NoContent();
//    }

//}