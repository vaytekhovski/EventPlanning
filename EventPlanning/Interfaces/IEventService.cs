using System;
using EventPlanning.Models;
using EventPlanning.Models.DTO;
using MongoDB.Bson;

namespace EventPlanning.Interfaces
{
	public interface IEventService
	{
        Task<Event> CreateEventAsync(EventCreateDto eventCreateDto);
        //Task<(bool Success, string Message)> RegisterForEventAsync(ObjectId eventId, ObjectId userId);
        Task<IEnumerable<Event>> GetAllEventsAsync();
        Task<Event> GetByIdAsync(ObjectId id);
        Task<bool> IsUserParticipatingAsync(ObjectId eventId, ObjectId userId);
        Task<(bool Success, string Message)> RequestParticipationAsync(ObjectId eventId, ObjectId userId);
        Task<bool> ConfirmParticipationAsync(ObjectId eventId, ObjectId userId, string confirmationCode);
        Task<bool> UpdateEventAsync(ObjectId eventId, EventUpdateDto eventUpdateDto);
        Task<bool> DeleteEventAsync(ObjectId eventId);
        Task<IEnumerable<User>> GetEventParticipantsAsync(ObjectId eventId);
        Task<bool> CancelParticipationAsync(ObjectId eventId, ObjectId userId);

    }
}

