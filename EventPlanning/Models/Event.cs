using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EventPlanning.Models
{
    public class Event
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; }
        public int MaxParticipants { get; set; }
        public List<ObjectId> ParticipantIds { get; set; } = new List<ObjectId>();
        public List<(ObjectId UserId, string ConfirmationCode)> PendingParticipantIds { get; set; } = new List<(ObjectId UserId, string ConfirmationCode)>();

        public bool RequiresPhoneConfirmation { get; set; } = false;
        public BsonDocument DynamicFields { get; set; }
    }
}

