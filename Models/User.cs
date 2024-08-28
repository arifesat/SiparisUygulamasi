using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text;
using System.Security.Cryptography;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; set; }

    [BsonElement("Username")]
    public required string Username { get; set; }

    [BsonElement("Email")]
    public required string Email { get; set; }

    private string _password;
    [BsonIgnore]
    public required string Password
    {
        get => _password;
        set
        {
            _password = value;
            PasswordHash = ComputeHash(_password);
        }
    }

    [BsonElement("PasswdHash")]
    public string PasswordHash { get; set; } // Şifre hash'i

    [BsonElement("Balance")]
    public required decimal Balance { get; set; } // Kullanıcının bakiyesi

    [BsonElement("Admin")]
    public bool IsAdmin { get; set; }

    private string ComputeHash(string input)
    {
        using (var sha256 = SHA256.Create())
        {
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            var builder = new StringBuilder();
            foreach (var b in bytes)
            {
                builder.Append(b.ToString("x2"));
            }
            return builder.ToString();
        }
    }
    public void SetPassword(string password)
    {
        Password = password;
    }
}
