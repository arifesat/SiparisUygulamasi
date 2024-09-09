using MongoDB.Bson;
using SiparisUygulamasi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SiparisUygulamasi.Repositories
{
    public interface IProductRepository
    {
        Task<List<Product>> GetProductsAsync();
        Task<Product> GetProductByIdAsync(ObjectId id);
        Task AddProductAsync(Product product);
        Task UpdateProductAsync(ObjectId id, Product updatedProduct);
        Task DeleteProductAsync(ObjectId id);
    }
}
