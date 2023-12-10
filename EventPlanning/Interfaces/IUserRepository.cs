using System;
using EventPlanning.Models;
using MongoDB.Bson;

namespace EventPlanning.Interfaces
{
	public interface IUserRepository
	{
        Task<bool> EmailExistsAsync(string email);
        Task<User> CreateAsync(User user);
        Task<User> GetByIdAsync(ObjectId id);
        Task<User> GetByEmailAsync(string email);
        Task<IEnumerable<User>> GetUsersByIdsAsync(IEnumerable<ObjectId> userIds);
    }
}

