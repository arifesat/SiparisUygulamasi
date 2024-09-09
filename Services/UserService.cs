using MongoDB.Bson;
using SiparisUygulamasi.Models;
using SiparisUygulamasi.Repositories;

namespace SiparisUygulamasi.Services
{
    public class UserService : IUserService
    {
        private readonly UserRepository _userRepository;

        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetUsersAsync();
        }

        public async Task<User> GetUserByIdAsync(ObjectId id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetUserByEmailAsync(email);
        }

        public async Task AddUserAsync(User user)
        {
            await _userRepository.AddUserAsync(user);
        }

        public async Task UpdateUserAsync(ObjectId id, User updatedUser)
        {
            await _userRepository.UpdateUserAsync(id, updatedUser);
        }

        public async Task DeleteUserAsync(ObjectId id)
        {
            await _userRepository.DeleteUserAsync(id);
        }
    }
}
