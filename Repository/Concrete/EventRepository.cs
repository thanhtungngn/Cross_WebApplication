using Cross_WebApplication.Context;
using Cross_WebApplication.Entities;
using Cross_WebApplication.Repository.Abstract;
using MongoDB.Driver;

namespace Cross_WebApplication.Repository.Concrete
{
    public class EventRepository : Repository<Event>, IEventRepository
    {
        public EventRepository(MongoDbContext context) : base(context, "Events") { }

        public async Task MarkAsProcessedAsync(string eventId, string processedBy)
        {
            var update = Builders<Event>.Update
                .Set(e => e.IsProcessed, true)
                .Set(e => e.ProcessedBy, processedBy)
                .Set(e => e.ProcessedAt, DateTime.UtcNow);

            await _collection.UpdateOneAsync(e => e.Id == new Guid(eventId), update);
        }
    }
}
