using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SiparisUygulamasi.Models;
using SiparisUygulamasi.Data;


namespace SiparisUygulamasi.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IOptions<MongoDBSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.DatabaseName);
        }
        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _database.GetCollection<T>(name);
        }

        public IMongoCollection<Product> Products => _database.GetCollection<Product>("Products");
        public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
        public IMongoCollection<Order> Orders => _database.GetCollection<Order>("Orders");
        public IMongoCollection<ShoppingCart> ShoppingCarts => _database.GetCollection<ShoppingCart>("ShoppingCarts");
    }
}
