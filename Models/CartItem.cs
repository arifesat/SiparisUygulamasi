using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace SiparisUygulamasi.Models
{
    public class CartItem
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; } // Sepet öğesi Id'si

        [BsonElement("ProductId")]
        public string ProductId { get; set; } // Sepet öğesinde yer alan ürünün Id'si

        [BsonElement("Product")]
        public Product Product { get; set; }

        [BsonElement("Quantity")]
        public int Quantity { get; set; } // Ürün miktarı

        [BsonElement("Price")]
        public decimal Price { get; set; } // Ürün fiyatı

        public decimal TotalPrice => Price * Quantity; // Ürün miktarı ile fiyatı çarpılarak toplam fiyat hesaplanır

    }
}
