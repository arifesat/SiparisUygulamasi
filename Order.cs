using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;



namespace SiparisUygulamasi.Models
{
    public class Order
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public ObjectId UserId { get; set; }  // Siparişi veren kullanıcının Id'si

        public List<OrderItem> Products { get; set; }  // Siparişte yer alan ürünler listesi

        public DateTime OrderDate { get; set; }

        public decimal TotalAmount { get; set; }

        public string Status { get; set; }  //Siparişin durumu
    }

    public class OrderItem
    {
        public ObjectId ProductId { get; set; }  // Siparişte yer alan ürünün Id'si
        public int Quantity { get; set; }        // Ürünün miktarı
    }
}
