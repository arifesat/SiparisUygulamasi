using Microsoft.AspNetCore.Mvc;
using SiparisUygulamasi.Data;
using SiparisUygulamasi.Models;
using MongoDB.Driver;
using MongoDB.Bson;

namespace SiparisUygulamasi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AddressController : ControllerBase
    {
        private readonly AddressService _addressService;

        public AddressController(AddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<List<Address>>> GetAddressesByUserId(string userId)
        {
            if (!ObjectId.TryParse(userId, out var objectId))
            {
                return BadRequest("Invalid user ID format.");
            }

            var addresses = await _addressService.GetAddressesByUserIdAsync(objectId);
            if (addresses == null || addresses.Count == 0)
            {
                return NotFound();
            }

            return addresses;
        }

        [HttpPost("{userId}")]
        public async Task<IActionResult> AddAddress(string userId, [FromBody] Address address)
        {
            if (!ObjectId.TryParse(userId, out var objectId))
            {
                return BadRequest("Invalid user ID format.");
            }

            address.UserId = objectId;
            await _addressService.AddAddressAsync(address);
            return CreatedAtAction(nameof(GetAddressesByUserId), new { userId = userId }, address);
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateAddress(string userId, [FromBody] Address updatedAddress)
        {
            if (!ObjectId.TryParse(userId, out var userObjectId))
            {
                return BadRequest("Invalid ID format.");
            }

            updatedAddress.UserId = userObjectId;
            await _addressService.UpdateAddressAsync(userObjectId, updatedAddress);
            return NoContent();
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteAddress(string userId)
        {
            if (!ObjectId.TryParse(userId, out var userObjectId))
            {
                return BadRequest("Invalid ID format.");
            }

            await _addressService.DeleteAddressAsync(userObjectId);
            return NoContent();
        }
    }
}
