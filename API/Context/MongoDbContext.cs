using Cross_WebApplication.Configuration;
using Cross_WebApplication.Entities;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System.Data;

namespace Cross_WebApplication.Context
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration configuration)
        {
            var mongoConfig = configuration.GetSection(nameof(MongoDbConfig)).Get<MongoDbConfig>();
            var client = new MongoClient(mongoConfig.ConnectionString);
            _database = client.GetDatabase(mongoConfig.Name);
        }

        public IMongoDatabase Database => _database;

        public IMongoCollection<Event> Events => _database.GetCollection<Event>("Events");
        public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
        public IMongoCollection<Role> Roles => _database.GetCollection<Role>("Roles");
    }
}
