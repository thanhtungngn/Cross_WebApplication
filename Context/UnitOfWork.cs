using Cross_WebApplication.Repository.Abstract;
using Cross_WebApplication.Repository.Concrete;

namespace Cross_WebApplication.Context
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MongoDbContext _context;

        public UnitOfWork(MongoDbContext context)
        {
            _context = context;
            Users = new UserRepository(context);
            Roles = new RoleRepository(context);
            Events = new EventRepository(context);
        }

        public IUserRepository Users { get; private set; }
        public IRoleRepository Roles { get; private set; }
        public IEventRepository Events { get; private set; }
       
    }

}
