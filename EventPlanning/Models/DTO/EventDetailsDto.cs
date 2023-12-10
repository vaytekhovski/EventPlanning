using System;

namespace EventPlanning.Models.DTO
{
	public class EventDetailsDto
	{
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; }

        public int MaxParticipants { get; set; }

        public bool RequiresPhoneConfirmation { get; set; } = false;
        public List<string> ParticipantIds { get; set; } = new List<string>();
        public Dictionary<string, object> DynamicFields { get; set; }
    }
}

