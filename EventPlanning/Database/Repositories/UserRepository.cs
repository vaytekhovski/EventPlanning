using System;
using EventPlanning.Interfaces;
using EventPlanning.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EventPlanning.Database.Repositories
{
	public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _usersCollection;

        public UserRepository(IMongoCollection<User> usersCollection)
		{
            _usersCollection = usersCollection;
        }

        public async Task<User> CreateAsync(User user)
        {
            await _usersCollection.InsertOneAsync(user);
            return user;
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            var count = await _usersCollection.CountDocumentsAsync(user => user.Email == email);
            return count > 0;
        }

        public async Task<User> GetByIdAsync(ObjectId id)
        {
            return await _usersCollection.Find(user => user.Id == id).FirstOrDefaultAsync();
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _usersCollection.Find(user => user.Email == email).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<User>> GetUsersByIdsAsync(IEnumerable<ObjectId> userIds)
        {
            var filter = Builders<User>.Filter.In(u => u.Id, userIds);
            var users = await _usersCollection.Find(filter).ToListAsync();
            return users;
        }

    }
}

