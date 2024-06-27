using Cross_WebApplication.Context;
using Cross_WebApplication.Entities;
using Cross_WebApplication.Repository.Abstract;
using CrossApplication.API.Models.DTO;
using CrossApplication.API.Models.Helper;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Cross_WebApplication.Repository.Concrete
{
    public class EventRepository : Repository<Event>, IEventRepository
    {
        public EventRepository(MongoDbContext context) : base(context, "Events") { }

        public async Task<PagedResponse<Event>> GetAllEventsAsync(EventRequestDto eventRequestDto)
        {
            // Build the filter definition
            var filter = Builders<Event>.Filter.Empty;
            if (!string.IsNullOrEmpty(eventRequestDto.SearchQuery))
            {
                filter = Builders<Event>.Filter.Or(
                    Builders<Event>.Filter.Regex("Title", new BsonRegularExpression(eventRequestDto.SearchQuery, "i")),
                    Builders<Event>.Filter.Regex("Description", new BsonRegularExpression(eventRequestDto.SearchQuery, "i"))
                );
            }

            // Sorting
            var sort = Builders<Event>.Sort.Ascending(eventRequestDto.SortOrder);
            if (!eventRequestDto.SortByAsc)
            {
                sort = Builders<Event>.Sort.Descending(eventRequestDto.SortOrder.Replace("_desc", ""));
            }

            // Pagination
            var pageSize = eventRequestDto.PageSize;
            var pageNumber = eventRequestDto.PageNumber;
            var totalItems = await _collection.CountDocumentsAsync(filter);
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            // Execute the query
            var eventsList = await _collection.Find(filter)
                .Sort(sort)
                .Skip((pageNumber - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();

            // Response with pagination
            var response = new PagedResponse<Event>
            {
                Data = eventsList,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages,
                TotalRecords = totalItems
            };
            return response;
        }

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
