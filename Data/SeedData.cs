using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
namespace SiparisUygulamasi.Data
{
    public class SeedData
    {
        private readonly MongoDBContext _context;

        public SeedData(MongoDBContext context)
        {
            _context = context;
        }

        //Database's name is ProductsDB and works on port 27017
        //Check appsettings.json

        public async Task SeedAsync()
        {
            // If any data is available in the given collection it doesn't add anything

            // Adds prodcuts to the "Produdcts" collection in db

            // Check if any products exist
            if (!_context.Products.Find(_ => true).Any())
            {
                var products = new List<Product>
                    {
                      new Product { Name = "Example Product", Price = 100.00M, Category = "Electronics", StockQuantity = 10 },
                      new Product { Name = "Another Product", Price = 50.00M, Category = "Books", StockQuantity = 20 },
                      new Product { Name = "Yet Another Product", Price = 500.00M, Category = "Clothes", StockQuantity = 200 }
                    };
                await _context.Products.InsertManyAsync(products);
            }

            // Adds users to the "Users" collection in db

            if(!_context.Users.Find(_ => true).Any())
            {
                var users = new List<User>
                {
                    new User {Username = "Arif", Email= "arif_esat@outlook.com", Password = "12345", Balance = 1000.00M, Role = "Admin"},
                    new User {Username = "Esat", Email= "arif_esat@outlook.com", Password = "123456", Balance = 10.00M, Role = "Customer"},
                    new User {Username = "Yilmaz", Email= "arif_esat@outlook.com", Password = "111111", Balance = 10.00M, Role = "Customer"}

                    };
                await _context.Users.InsertManyAsync(users);
            }
        }
    }
}

