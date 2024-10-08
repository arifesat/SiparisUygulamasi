﻿using MongoDB.Bson;
using MongoDB.Driver;
using SiparisUygulamasi.Data;
using SiparisUygulamasi.Models;

namespace SiparisUygulamasi.Repositories
{
    public class UserRepository : IUserRepository<User>
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

        public async Task<User> GetByNameAsync(string userName)
        {
            return await _users.Find(user => user.Username == userName).FirstOrDefaultAsync();
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _users.Find(user => user.Email == email).FirstOrDefaultAsync();
        }

        public async Task<User> UpdateUserAsync(object id, User entity)
        {
            var objectId = (ObjectId)id;
            await _users.ReplaceOneAsync(user => user.Id == objectId, entity);
            return entity;
        }

        public async Task<User> UpdateUserBookAsync(object id, User entity)
        {
            var objectId = (ObjectId)id;
            await _users.ReplaceOneAsync(user => user.Id == objectId, entity);
            return entity;
        }

        public async Task<User> GetUserById(object _id)
        {
            var objectId = (ObjectId)_id;
            return await _users.Find(user => user.Id == objectId).FirstOrDefaultAsync();
        }
    }
}