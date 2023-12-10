using System;
using System.Linq;
using EventPlanning.Interfaces;
using EventPlanning.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EventPlanning.Database.Repositories
{
	public class EventRepository : IEventRepository
	{
        private readonly IMongoCollection<Event> _eventsCollection;

        public EventRepository(IMongoCollection<Event> eventsCollection)
        {
            _eventsCollection = eventsCollection;
        }

        public async Task<Event> CreateAsync(Event eventItem)
        {
            var existingEvent = await _eventsCollection
            .Find(e => e.Title == eventItem.Title)
            .FirstOrDefaultAsync();

            if (existingEvent != null)
            {
                throw new ArgumentException("Событие с таким названием уже существует.");
            }

            await _eventsCollection.InsertOneAsync(eventItem);
            return eventItem;
        }

        public async Task<IEnumerable<Event>> GetAllAsync()
        {
            return await _eventsCollection.Find(_ => true).ToListAsync();
        }

        public async Task<Event> GetByIdAsync(ObjectId id)
        {
            return await _eventsCollection.Find(e => e.Id == id).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateAsync(Event eventItem)
        {
            var filter = Builders<Event>.Filter.Eq(e => e.Id, eventItem.Id);
            var updateResult = await _eventsCollection.ReplaceOneAsync(filter, eventItem);

            if (updateResult.MatchedCount == 0)
            {
                throw new KeyNotFoundException("Событие не найдено.");
            }

            return updateResult.ModifiedCount > 0;
        }

        public async Task<bool> IsUserParticipating(ObjectId eventId, ObjectId userId)
        {
            var filter = Builders<Event>.Filter.Eq(e => e.Id, eventId);
            var eventItem = await _eventsCollection.Find(filter).FirstOrDefaultAsync();

            if (eventItem == null)
            {
                throw new KeyNotFoundException("Событие не найдено.");
            }

            return eventItem.ParticipantIds.Contains(userId);
        }

        public async Task<bool> DeleteAsync(ObjectId eventId)
        {
            var result = await _eventsCollection.DeleteOneAsync(e => e.Id == eventId);
            return result.DeletedCount > 0;
        }

    }
}

