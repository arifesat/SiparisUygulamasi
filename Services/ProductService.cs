using MongoDB.Bson;
using SiparisUygulamasi.Repositories;
namespace SiparisUygulamasi.Services

{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _productRepository.GetProductsAsync();
        }

        public async Task<Product> GetProductByIdAsync(ObjectId id)
        {
            return await _productRepository.GetProductByIdAsync(id);
        }

        public async Task AddProductAsync(Product product)
        {
            await _productRepository.AddProductAsync(product);
        }

        public async Task UpdateProductAsync(ObjectId id, Product updatedProduct)
        {
            await _productRepository.UpdateProductAsync(id, updatedProduct);
        }

        public async Task DeleteProductAsync(ObjectId id)
        {
            await _productRepository.DeleteProductAsync(id);
        }
    }
}
