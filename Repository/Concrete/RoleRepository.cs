using Cross_WebApplication.Context;
using Cross_WebApplication.Entities;
using Cross_WebApplication.Repository.Abstract;

namespace Cross_WebApplication.Repository.Concrete
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(MongoDbContext context) : base(context, "Roles") { }
    }
}
