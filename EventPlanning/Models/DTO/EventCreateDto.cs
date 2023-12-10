using System;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace EventPlanning.Models.DTO
{
    public class EventCreateDto
    {
        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Title { get; set; }
        [Required]
        [StringLength(500)]
        public string Description { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [StringLength(200)]
        public string Location { get; set; }

        public int MaxParticipants { get; set; }

        public bool RequiresPhoneConfirmation { get; set; } = false;
        public Dictionary<string, string> DynamicFields { get; set; }
    }
}

