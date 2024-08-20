using Microsoft.AspNetCore.Mvc;
using SiparisUygulamasi.Data;
using SiparisUygulamasi.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;



[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly MongoDBContext _context;

    public ProductsController(MongoDBContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IEnumerable<Product>> Get()
    {
        return await _context.Products.Find(_ => true).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> Get(string id)
    {
        var product = await _context.Products.Find(p => p.Id == id).FirstOrDefaultAsync();

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
        return CreatedAtRoute(new { id = product.Id }, product);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, Product productIn)
    {
        var product = await _context.Products.Find(p => p.Id == id).FirstOrDefaultAsync();

        if (product == null)
        {
            return NotFound();
        }

        await _context.Products.ReplaceOneAsync(p => p.Id == id, productIn);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var product = await _context.Products.Find(p => p.Id == id).FirstOrDefaultAsync();

        if (product == null)
        {
            return NotFound();
        }

        await _context.Products.DeleteOneAsync(p => p.Id == id);

        return NoContent();
    }


}
