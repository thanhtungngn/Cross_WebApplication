using Cross_WebApplication.Context;
using Cross_WebApplication.Entities;
using Cross_WebApplication.Repository.Abstract;

namespace Cross_WebApplication.Repository.Concrete
{
    public class EventRepository : Repository<Event>, IEventRepository
    {
        public EventRepository(MongoDbContext context) : base(context, "Events") { }
    }
}
