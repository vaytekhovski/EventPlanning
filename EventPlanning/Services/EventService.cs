using System;
using EventPlanning.Interfaces;
using EventPlanning.Models;
using EventPlanning.Models.DTO;
using EventPlanning.Services.Messages;
using MongoDB.Bson;

namespace EventPlanning.Services
{
	public class EventService : IEventService
	{
        private readonly IEventRepository _eventRepository;
        private readonly IUserRepository _userRepository;
        private readonly MessageServiceFactory _messageServiceFactory;

        public EventService(IEventRepository eventRepository, IUserRepository userRepository, MessageServiceFactory messageServiceFactory)
        {
            _eventRepository = eventRepository;
            _userRepository = userRepository;
            _messageServiceFactory = messageServiceFactory;
        }

        public async Task<Event> CreateEventAsync(EventCreateDto eventCreateDto)
        {
            if (eventCreateDto.Date <= DateTime.Now)
            {
                throw new ArgumentException("Дата события должна быть в будущем.");
            }

            var bsonDocument = new BsonDocument();
            foreach (var item in eventCreateDto.DynamicFields)
            {
                bsonDocument[item.Key] = new BsonString(item.Value);
            }

            var newEvent = new Event
            {
                Title = eventCreateDto.Title,
                Description = eventCreateDto.Description,
                Date = eventCreateDto.Date,
                Location = eventCreateDto.Location,
                MaxParticipants = eventCreateDto.MaxParticipants,
                RequiresPhoneConfirmation = eventCreateDto.RequiresPhoneConfirmation,
                DynamicFields = bsonDocument
            };

            return await _eventRepository.CreateAsync(newEvent);
        }

        public async Task<IEnumerable<Event>> GetAllEventsAsync()
        {
            return await _eventRepository.GetAllAsync();
        }

        public async Task<Event> GetByIdAsync(ObjectId id)
        {
            return await _eventRepository.GetByIdAsync(id);
        }

        //public async Task<(bool Success, string Message)> RegisterForEventAsync(ObjectId eventId, ObjectId userId)
        //{
        //    var eventItem = await _eventRepository.GetByIdAsync(eventId);

        //    if (eventItem == null)
        //    {
        //        return (false, "Событие не найдено.");
        //    }

        //    if (eventItem.ParticipantIds.Contains(userId))
        //    {
        //        return (false, "Пользователь уже зарегистрирован на это событие.");
        //    }

        //    if (eventItem.ParticipantIds.Count >= eventItem.MaxParticipants)
        //    {
        //        return (false, "Достигнуто максимальное количество участников.");
        //    }

        //    eventItem.ParticipantIds.Add(userId);
        //    await _eventRepository.UpdateAsync(eventItem);
        //    return (true, "Регистрация на событие успешно выполнена.");
        //}

        public async Task<(bool Success, string Message)> RequestParticipationAsync(ObjectId eventId, ObjectId userId)
        {
            var eventItem = await _eventRepository.GetByIdAsync(eventId);

            if (eventItem == null)
            {
                return (false, "Событие не найдено.");
            }

            if (eventItem.ParticipantIds.Contains(userId))
            {
                return (false, "Пользователь уже зарегистрирован на это событие.");
            }

            if (eventItem.ParticipantIds.Count >= eventItem.MaxParticipants)
            {
                return (false, "Достигнуто максимальное количество участников.");
            }

            if (eventItem.PendingParticipantIds.Any(participant => participant.UserId == userId))
            {
                return (false, "Пользователь уже отправил запрос на участие.");
            }

            var confirmationCode = GenerateConfirmationCode();

            eventItem.PendingParticipantIds.Add((userId, confirmationCode));
            await _eventRepository.UpdateAsync(eventItem);

            /*Если не успользуется реальный SMTP или SMS отправитель оставить эти строчки закоментированными*/

            //IMessageService messageService = eventItem.RequiresPhoneConfirmation
            //    ? _messageServiceFactory.GetService(MessageType.SMS)
            //    : _messageServiceFactory.GetService(MessageType.Email);

            //var user = await _userRepository.GetByIdAsync(userId);
            //await messageService.SendMessageAsync(user.Email, confirmationCode);

            return (true, "Запрос на регистрацию успешно выполнен.");
        }

        public async Task<bool> ConfirmParticipationAsync(ObjectId eventId, ObjectId userId, string confirmationCode)
        {
            var eventItem = await _eventRepository.GetByIdAsync(eventId);

            var pendingEntry = eventItem.PendingParticipantIds.FirstOrDefault(p => p.UserId == userId && p.ConfirmationCode == confirmationCode);
            if (pendingEntry == default)
                return false;

            eventItem.PendingParticipantIds.Remove(pendingEntry);
            eventItem.ParticipantIds.Add(pendingEntry.UserId);
            await _eventRepository.UpdateAsync(eventItem);

            return true;
        }

        public async Task<bool> IsUserParticipatingAsync(ObjectId eventId, ObjectId userId)
        {
            return await _eventRepository.IsUserParticipating(eventId, userId);
        }

        public async Task<bool> UpdateEventAsync(ObjectId eventId, EventUpdateDto eventUpdateDto)
        {
            var eventItem = await _eventRepository.GetByIdAsync(eventId);
            if (eventItem == null)
            {
                return false;
            }

            eventItem.Title = eventUpdateDto.Title ?? eventItem.Title;
            eventItem.Description = eventUpdateDto.Description ?? eventItem.Description;
            eventItem.Date = eventUpdateDto.Date ?? eventItem.Date;
            eventItem.Location = eventUpdateDto.Location ?? eventItem.Location;
            eventItem.MaxParticipants = eventUpdateDto.MaxParticipants ?? eventItem.MaxParticipants;
            eventItem.RequiresPhoneConfirmation = eventUpdateDto.RequiresPhoneConfirmation ?? eventItem.RequiresPhoneConfirmation;
            var bsonDocument = new BsonDocument();
            foreach (var item in eventUpdateDto.DynamicFields)
            {
                bsonDocument[item.Key] = new BsonString(item.Value);
            }
            eventItem.DynamicFields = bsonDocument ?? eventItem.DynamicFields;

            await _eventRepository.UpdateAsync(eventItem);
            return true;
        }

        public async Task<bool> DeleteEventAsync(ObjectId eventId)
        {
            return await _eventRepository.DeleteAsync(eventId);
        }

        public async Task<IEnumerable<User>> GetEventParticipantsAsync(ObjectId eventId)
        {
            var eventItem = await _eventRepository.GetByIdAsync(eventId);
            if (eventItem == null)
            {
                return null;
            }

            var participantIds = eventItem.ParticipantIds;
            return await _userRepository.GetUsersByIdsAsync(participantIds);
        }

        public async Task<bool> CancelParticipationAsync(ObjectId eventId, ObjectId userId)
        {
            var eventItem = await _eventRepository.GetByIdAsync(eventId);
            if (eventItem == null || !eventItem.ParticipantIds.Contains(userId))
            {
                return false;
            }

            eventItem.ParticipantIds.Remove(userId);
            await _eventRepository.UpdateAsync(eventItem);

            return true;
        }




        private string GenerateConfirmationCode()
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var part1 = new string(Enumerable.Repeat(chars, 3).Select(s => s[random.Next(s.Length)]).ToArray());
            var part2 = new string(Enumerable.Repeat(chars, 3).Select(s => s[random.Next(s.Length)]).ToArray());

            return $"{part1}-{part2}";
        }
    }
}

