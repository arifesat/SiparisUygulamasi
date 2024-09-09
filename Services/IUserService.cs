using MongoDB.Bson;

namespace SiparisUygulamasi.Services
{
    public interface IUserService
    {
        Task<List<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(ObjectId id);
        Task<User> GetUserByEmailAsync(string email);
        //Task<User> GetUserByUsernameAsync(string username);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(ObjectId id, User updatedUser);
        Task DeleteUserAsync(ObjectId id);
    }
}
