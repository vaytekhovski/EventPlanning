using System;
using MongoDB.Bson;

namespace EventPlanning.Models.DTO
{
    public class EventUpdateDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? Date { get; set; }
        public string Location { get; set; }

        public int? MaxParticipants { get; set; }

        public bool? RequiresPhoneConfirmation { get; set; } = false;
        public Dictionary<string, string>? DynamicFields { get; set; }
    }

}

