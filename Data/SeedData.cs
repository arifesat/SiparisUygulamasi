using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using SiparisUygulamasi.Data;
namespace SiparisUygulamasi
{
    internal class Seed_data
    {
        public class SeedData
        {
            private readonly MongoDBContext _context;

            public SeedData(MongoDBContext context)
            {
                _context = context;
            }

            public async Task SeedAsync()
            {
                // Check if any products exist
                if (!_context.Products.Find(_ => true).Any())
                {
                    var products = new List<Product>
            {
                new Product { Name = "Example Product", Price = 100.00M, Category = "Electronics", StockQuantity = 10 },
                new Product { Name = "Another Product", Price = 50.00M, Category = "Books", StockQuantity = 20 }
            };

                    await _context.Products.InsertManyAsync(products);
                }

                // Similar logic can be applied to Users and Orders
            }
        }

    }
}
