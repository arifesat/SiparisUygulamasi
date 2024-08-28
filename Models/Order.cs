using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SiparisUygulamasi.Models;

public class Order
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; set; } // Sipariş Id'si

    [BsonElement("UserId")]
    public ObjectId UserId { get; set; }  // Siparişi veren kullanıcının Id'si

    [BsonElement("Items")]
    public List<CartItem> Items { get; set; }  = new List<CartItem>();  // Siparişte yer alan ürünler listesi

    [BsonElement("OrderDate")]
    public DateTime OrderDate { get; set; } // Siparişin verildiği tarih

    [BsonElement("TotalAmount")]
    public decimal TotalAmount { get; set; } // Siparişin toplam tutarı

    [BsonElement("Address")]
    public Address Address { get; set; } = new Address(); // Siparişin teslim edileceği adres
}