using MongoDB.Bson;
using MongoDB.Driver;
using SiparisUygulamasi.Models;

namespace SiparisUygulamasi.Data
{
    public class ProductRepository
    {
        private readonly IMongoCollection<Product> _products;

        public ProductRepository(MongoDBContext context)
        {
            _products = context.Products;
        }

        // Method to get all products
        public async Task<List<Product>> GetProductsAsync()
        {
            return await _products.Find(product => true).ToListAsync();
        }

        // Method to get a product by its ID
        public async Task<Product> GetProductByIdAsync(ObjectId id)
        {
            return await _products.Find(product => product.Id == id.ToString()).FirstOrDefaultAsync();
        }

        // Method to add a new product
        public async Task AddProductAsync(Product product)
        {
            await _products.InsertOneAsync(product);
        }

        // Method to update an existing product
        public async Task UpdateProductAsync(ObjectId id, Product updatedProduct)
        {
            await _products.ReplaceOneAsync(product => product.Id == id.ToString(), updatedProduct);
        }

        // Method to delete a product
        public async Task DeleteProductAsync(ObjectId id)
        {
            await _products.DeleteOneAsync(product => product.Id == id.ToString());
        }
    }
}
