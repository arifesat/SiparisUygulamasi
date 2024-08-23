using MongoDB.Bson;
using SiparisUygulamasi.Models;
using SiparisUygulamasi.Repositories;

public class AddressService
{
    private readonly AddressRepository _addressRepository;

    public AddressService(AddressRepository addressRepository)
    {
        _addressRepository = addressRepository;
    }

    public async Task<List<Address>> GetAllAddressesAsync()
    {
        return await _addressRepository.GetAddressesAsync();
    }

    public async Task<Address> GetAddressByIdAsync(ObjectId id)
    {
        return await _addressRepository.GetAddressByIdAsync(id);
    }

    public async Task AddAddressAsync(Address address)
    {
        await _addressRepository.AddAddressAsync(address);
    }

    public async Task UpdateAddressAsync(ObjectId id, Address updatedAddress)
    {
        await _addressRepository.UpdateAddressAsync(id, updatedAddress);
    }

    public async Task DeleteAddressAsync(ObjectId id)
    {
        await _addressRepository.DeleteAddressAsync(id);
    }

    public async Task<List<Address>> GetAddressesByUserIdAsync(ObjectId userId)
    {
        return await _addressRepository.GetAddressesByUserIdAsync(userId);
    }
}