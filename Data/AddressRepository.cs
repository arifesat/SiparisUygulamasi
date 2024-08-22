using MongoDB.Bson;
using MongoDB.Driver;
using SiparisUygulamasi.Models;
using SiparisUygulamasi.Data;


namespace SiparisUygulamasi.Data
{
    public class AddressRepository
    {
        private readonly IMongoCollection<Address> _addresses;

        public AddressRepository(MongoDBContext context)
        {
            _addresses = context.Addresses;
        }

        public async Task<List<Address>> GetAddressesAsync()
        {
            return await _addresses.Find(address => true).ToListAsync();
        }

        public async Task<Address> GetAddressByIdAsync(ObjectId id)
        {
            return await _addresses.Find(address => address.Id == id).FirstOrDefaultAsync();
        }

        public async Task AddAddressAsync(Address address)
        {
            await _addresses.InsertOneAsync(address);
        }

        public async Task UpdateAddressAsync(ObjectId id, Address updatedAddress)
        {
            await _addresses.ReplaceOneAsync(address => address.Id == id, updatedAddress);
        }

        public async Task DeleteAddressAsync(ObjectId id)
        {
            await _addresses.DeleteOneAsync(address => address.Id == id);
        }

        public async Task<List<Address>> GetAddressesByUserIdAsync(ObjectId userId)
        {
            return await _addresses.Find(address => address.UserId == userId).ToListAsync();
        }
    }
}
