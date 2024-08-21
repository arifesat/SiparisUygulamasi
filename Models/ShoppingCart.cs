using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SiparisUygulamasi.Models
{
    public class ShoppingCart
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } // Sepet Id'si

        [BsonElement("UserId")]
        public string UserId { get; set; } // Sepeti oluşturan kullanıcının Id'si

        [BsonElement("Products")]
        public string ShoppingCartId { get; set; } // Sepette yer alan ürünler listesi

        [BsonElement("TotalAmount")]
        public decimal TotalAmount { get; set; } // Sepetin toplam tutarı

        [BsonElement("Status")]
        public string Status { get; set; } // Sepetin durumu
    }
}
