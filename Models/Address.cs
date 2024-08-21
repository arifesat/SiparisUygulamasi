using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace SiparisUygulamasi.Models
{
    public class Address
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Street")]
        public string Street { get; set; }

        [BsonElement("BuildingNo")]
        public string BuildingNo { get; set; }

        [BsonElement("DoorNo")]
        public string DoorNo { get; set; }

        [BsonElement("City")]
        public string City { get; set; }

        [BsonElement("PostalCode")]
        public string PostalCode { get; set; }

        [BsonElement("Country")]
        public string Country { get; set; }

    }
}
