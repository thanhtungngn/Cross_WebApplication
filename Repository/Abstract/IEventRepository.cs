using Cross_WebApplication.Entities;

namespace Cross_WebApplication.Repository.Abstract
{
    public interface IEventRepository : IRepository<Event>
    {
        Task MarkAsProcessedAsync(string eventId, string processedBy);
    }
}
