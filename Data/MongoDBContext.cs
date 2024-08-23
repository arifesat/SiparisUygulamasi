using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using SiparisUygulamasi.Models;

namespace SiparisUygulamasi.Data
{
    public class MongoDBContext
    {
        private readonly IMongoDatabase _database;

        public MongoDBContext(IOptions<MongoDBSettings> settings)

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
        public IMongoCollection<Address> Addresses => _database.GetCollection<Address>("Addreses");

        public IMongoDatabase Database => _database;
    }

}
