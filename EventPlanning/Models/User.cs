using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EventPlanning.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        [BsonRepresentation(BsonType.String)]
        public UserRole Role { get; set; } = UserRole.Basic;
    }

}

