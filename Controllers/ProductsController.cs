using Microsoft.AspNetCore.Mvc;
using SiparisUygulamasi.Data;
using SiparisUygulamasi.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly MongoDBContext _context;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(MongoDBContext context, ILogger<ProductsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    //[HttpGet]
    //public async Task<IEnumerable<Product>> Get()
    //{
    //    try
    //    {
    //        return await _context.Products.Find(_ => true).ToListAsync();
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError(ex, "An error occurred while retrieving products.");
    //        throw;
    //    }
    //}

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> Get(string id)
    {
        if (!ObjectId.TryParse(id, out var objectId))
        {
            return BadRequest("Invalid ID format.");
        }

        var product = await _context.Products.Find(p => p.Id == objectId).FirstOrDefaultAsync();

        if (product == null)
        {
            return NotFound();
        }

        return product;
    }

    [HttpPost]
    public async Task<ActionResult<Product>> Create(Product product)
    {
        await _context.Products.InsertOneAsync(product);
        return CreatedAtRoute(new { id = product.Id.ToString() }, product);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, Product productIn)
    {
        if (!ObjectId.TryParse(id, out var objectId))
        {
            return BadRequest("Invalid ID format.");
        }

        var product = await _context.Products.Find(p => p.Id == objectId).FirstOrDefaultAsync();

        if (product == null)
        {
            return NotFound();
        }

        await _context.Products.ReplaceOneAsync(p => p.Id == objectId, productIn);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        if (!ObjectId.TryParse(id, out var objectId))
        {
            return BadRequest("Invalid ID format.");
        }

        var product = await _context.Products.Find(p => p.Id == objectId).FirstOrDefaultAsync();

        if (product == null)
        {
            return NotFound();
        }

        await _context.Products.DeleteOneAsync(p => p.Id == objectId);

        return NoContent();
    }
}
