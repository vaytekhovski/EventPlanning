using System;
using EventPlanning.Models;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace EventPlanning.Interfaces
{
    public interface IEventRepository
    {
        Task<Event> CreateAsync(Event eventItem);
        Task<IEnumerable<Event>> GetAllAsync();
        Task<Event> GetByIdAsync(ObjectId id);
        Task<bool> UpdateAsync(Event eventItem);
        Task<bool> IsUserParticipating(ObjectId eventId, ObjectId userId);
        Task<bool> DeleteAsync(ObjectId eventId);
    }
}

