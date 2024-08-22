using MongoDB.Bson;
using MongoDB.Driver;
using SiparisUygulamasi.Models;

namespace SiparisUygulamasi.Data
{
    public class UserRepository
    {
        private readonly IMongoCollection<User> _users;

        public UserRepository(MongoDBContext context)
        {
            _users = context.Users;
        }

        public async Task<List<User>> GetUsersAsync()
        {
            return await _users.Find(user => true).ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(ObjectId id)
        {
            return await _users.Find(user => user.Id == id).FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByEmailAsync(string Email)
        {
            return await _users.Find(user => user.Email == Email).FirstOrDefaultAsync();
        }

        public async Task AddUserAsync(User user)
        {
            await _users.InsertOneAsync(user);
        }

        public async Task UpdateUserAsync(ObjectId id, User updatedUser)
        {
            await _users.ReplaceOneAsync(user => user.Id == id, updatedUser);
        }

        public async Task DeleteUserAsync(ObjectId id)
        {
            await _users.DeleteOneAsync(user => user.Id == id);
        }
    }
}
