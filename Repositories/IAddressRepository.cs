using MongoDB.Bson;
using SiparisUygulamasi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SiparisUygulamasi.Repositories
{
    public interface IAddressRepository
    {
        Task<List<Address>> GetAddressesAsync();
        Task<Address> GetAddressByIdAsync(ObjectId id);
        Task AddAddressAsync(Address address);
        Task UpdateAddressAsync(ObjectId userId, Address updatedAddress);
        Task DeleteAddressAsync(ObjectId userId);
        Task<List<Address>> GetAddressesByUserIdAsync(ObjectId userId);
    }
}
