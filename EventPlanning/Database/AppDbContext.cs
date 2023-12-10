using System;
using EventPlanning.Models;
using MongoDB.Driver;

namespace EventPlanning.Database
{
	public class AppDbContext
	{
        private readonly IMongoDatabase _database;

        public AppDbContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration["MongoDbSettings:ConnectionString"]);
            _database = client.GetDatabase(configuration["MongoDbSettings:DatabaseName"]);
        }

        public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
        public IMongoCollection<Event> Events => _database.GetCollection<Event>("Events");
    }
}

