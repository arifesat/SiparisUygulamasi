using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using SiparisUygulamasi.Models;

namespace SiparisUygulamasi.Data
{
    public class MongoDBContext
    {
        private readonly IMongoDatabase _database;

        //public MongoDBContext(string connectionString, string databaseName)
        public MongoDBContext(IOptions<MongoDBSettings> settings)

        {
            //var client = new MongoClient(connectionString);
            var client = new MongoClient(settings.Value.ConnectionString);

            //_database = client.GetDatabase(databaseName);
            _database = client.GetDatabase(settings.Value.DatabaseName);

        }

        public IMongoCollection<Product> Products => _database.GetCollection<Product>("Products");
        public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
        public IMongoCollection<Order> Orders => _database.GetCollection<Order>("Orders");
        public IMongoCollection<ShoppingCart> ShoppingCarts => _database.GetCollection<ShoppingCart>("ShoppingCarts");
        public IMongoDatabase Database => _database;
    }

}
