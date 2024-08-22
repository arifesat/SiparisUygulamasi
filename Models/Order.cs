using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SiparisUygulamasi.Models;

public class Order
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public String Id { get; set; } // Sipariş Id'si

    [BsonElement("UserId")]
    public String UserId { get; set; }  // Siparişi veren kullanıcının Id'si

    [BsonElement("Items")]
    public List<OrderItem> Items { get; set; }  // Siparişte yer alan ürünler listesi

    [BsonElement("OrderDate")]
    public DateTime OrderDate { get; set; } // Siparişin verildiği tarih

    [BsonElement("TotalAmount")]
    public decimal TotalAmount { get; set; } // Siparişin toplam tutarı

    [BsonElement("Status")]
    public string Status { get; set; }  //Siparişin durumu

    [BsonElement("Address")]
    public Address Address { get; set; }  // Siparişin teslim edileceği adres
}