using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SiparisUygulamasi.Models
{
    public class CartItem
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        [BsonElement("ProductId")]
        public ObjectId ProductId { get; set; }

        [BsonElement("Product")]
        public string Product { get; set; }

        [BsonElement("Quantity")]
        public int Quantity { get; set; }

        [BsonElement("Price")]
        public decimal Price { get; set; }

        [BsonIgnore]
        public decimal TotalPrice => Quantity * Price; // This property is calculated and should not be serialized
    }
}
