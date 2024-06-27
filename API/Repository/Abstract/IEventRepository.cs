using Cross_WebApplication.Entities;
using CrossApplication.API.Models.DTO;
using CrossApplication.API.Models.Helper;

namespace Cross_WebApplication.Repository.Abstract
{
    public interface IEventRepository : IRepository<Event>
    {
        Task MarkAsProcessedAsync(string eventId, string processedBy);
        Task<PagedResponse<Event>> GetAllEventsAsync(EventRequestDto eventRequestDto);
    }
}
