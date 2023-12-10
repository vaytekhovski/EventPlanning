using System;
using MongoDB.Bson;

namespace EventPlanning.Models.Extensions
{
    public static class BsonDocumentExtensions
    {
        public static Dictionary<string, object> ToDictionary(this BsonDocument bsonDocument)
        {
            var dictionary = new Dictionary<string, object>();
            foreach (var element in bsonDocument.Elements)
            {
                dictionary[element.Name] = element.Value.ToString();
            }
            return dictionary;
        }
    }

}

