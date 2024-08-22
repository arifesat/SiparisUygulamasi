using Microsoft.AspNetCore.Mvc;
using SiparisUygulamasi.Data;
using SiparisUygulamasi.Models;
using MongoDB.Driver;
using MongoDB.Bson;

namespace SiparisUygulamasi.Controllers
{
    public class AddressController : ControllerBase
    {
        private readonly MongoDBContext _context;
        private readonly ILogger<AddressController> _logger;

        public AddressController(MongoDBContext context, ILogger<AddressController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<Address>> Get()
        {
            try
            {
                return await _context.Addresses.Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving addresses.");
                throw;
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Address>> Get(string id)
        {
            var address = await _context.Addresses.Find(p => p.Id == id).FirstOrDefaultAsync();

            if (address == null)
            {
                return NotFound();
            }

            return address;
        }

        [HttpPost]
        public async Task<ActionResult<Address>> Create(Address address)
        {
            await _context.Addresses.InsertOneAsync(address);
            return CreatedAtRoute(new { id = address.Id }, address);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Address addressIn)
        {
            var address = await _context.Addresses.Find(p => p.Id == id).FirstOrDefaultAsync();
            if (address == null)
            {
                return NotFound();
            }

            await _context.Addresses.ReplaceOneAsync(p => p.Id == id, addressIn);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var address = await _context.Addresses.Find(p => p.Id == id).FirstOrDefaultAsync();
            if (address == null)
            {
                return NotFound();
            }

            await _context.Addresses.DeleteOneAsync(p => p.Id == id);
            return NoContent();
        }
    }
}
