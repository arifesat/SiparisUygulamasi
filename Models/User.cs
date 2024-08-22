using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; set; }

    [BsonElement("Username")]
    public string Username { get; set; }

    [BsonElement("Email")]
    public string Email { get; set; }

    [BsonElement("PasswdHash")]
    public string PasswordHash { get; set; } // Şifre hash'i

    [BsonElement("Balance")]
    public decimal Balance { get; set; } // Kullanıcının bakiyesi

    [BsonElement("Role")]
    public string Role { get; set; } // Kullanıcının rolü (Admin, Customer vb.)
}
