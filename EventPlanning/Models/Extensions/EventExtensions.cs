using System;
using EventPlanning.Models.DTO;

namespace EventPlanning.Models.Extensions
{
    public static class EventExtensions
    {
        public static EventDetailsDto ToDto(this Event eventItem)
        {
            return new EventDetailsDto
            {
                Id = eventItem.Id.ToString(),
                Title = eventItem.Title,
                Description = eventItem.Description,
                Date = eventItem.Date,
                Location = eventItem.Location,
                MaxParticipants = eventItem.MaxParticipants,
                ParticipantIds = eventItem.ParticipantIds.Select(id => id.ToString()).ToList(),
                RequiresPhoneConfirmation = eventItem.RequiresPhoneConfirmation,
                DynamicFields = eventItem.DynamicFields.ToDictionary()
            };
        }
    }
}


