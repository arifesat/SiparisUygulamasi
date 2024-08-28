using MongoDB.Bson;
using MongoDB.Driver;
using SiparisUygulamasi.Models;
using SiparisUygulamasi.Data;


namespace SiparisUygulamasi.Repositories
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

        public async Task UpdateAddressAsync(ObjectId userId, Address updatedAddress)
        {
            var update = Builders<Address>.Update
                .Set(a => a.Street, updatedAddress.Street)
                .Set(a => a.City, updatedAddress.City)
                .Set(a => a.PostalCode, updatedAddress.PostalCode)
                .Set(a => a.Country, updatedAddress.Country)
                .Set(a => a.BuildingNo, updatedAddress.BuildingNo)
                .Set(a => a.DoorNo, updatedAddress.DoorNo);

            await _addresses.UpdateOneAsync(address => address.UserId == userId, update);
            //await _addresses.ReplaceOneAsync(address => address.UserId == userId, updatedAddress);
        }

        public async Task DeleteAddressAsync(ObjectId userId)
        {
            await _addresses.DeleteOneAsync(address => address.UserId == userId);
        }

        public async Task<List<Address>> GetAddressesByUserIdAsync(ObjectId userId)
        {
            return await _addresses.Find(address => address.UserId == userId).ToListAsync();
        }
    }
}
