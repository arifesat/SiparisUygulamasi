using MongoDB.Bson;
using SiparisUygulamasi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SiparisUygulamasi.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(ObjectId id);
        Task AddProductAsync(Product product);
        Task UpdateProductAsync(ObjectId id, Product updatedProduct);
        Task DeleteProductAsync(ObjectId id);
    }
}
