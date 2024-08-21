using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SiparisUygulamasi.Models
{
    public class OrderItem
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public String Id { get; set; }

        [BsonElement("ProductId")]
        public string ProductId { get; set; }

        [BsonElement("ProductName")]
        public string ProductName { get; set; }

        [BsonElement("Quantity")]
        public int Quantity { get; set; }

        [BsonElement("Price")]
        public decimal Price { get; set; }
    }
}