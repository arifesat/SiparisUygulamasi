using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SiparisUygulamasi.Models;

public class ShoppingCart
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; set; } // Sepet Id'si

    [BsonElement("UserId")]
    public ObjectId UserId { get; set; } // Sepeti oluşturan kullanıcının Id'si

    //[BsonElement("Products")]
    //public ObjectId ShoppingCartId { get; set; } // Sepette yer alan ürünler listesi

    //[BsonElement("TotalAmount")]
    //public decimal TotalAmount { get; set; } // Sepetin toplam tutarı

    [BsonElement("Status")]
    public string Status { get; set; } // Sepetin durumu

    [BsonElement("TotalAmount")]
    public decimal TotalAmount => Items.Sum(item => item.TotalPrice);

    [BsonElement("Items")]
    public List<CartItem> Items { get; set; } = new List<CartItem>();

}


