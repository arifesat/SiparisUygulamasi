using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public String Id { get; set; }
    [BsonElement("Username")]
    public string Username { get; set; }

    [BsonElement("Email")]
    public string Email { get; set; }

    [BsonElement("PasswdHash")]
    public string PasswordHash { get; set; }

    [BsonElement("Role")]
    public string Role { get; set; }
}
