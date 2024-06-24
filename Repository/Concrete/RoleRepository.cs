using Cross_WebApplication.Context;
using Cross_WebApplication.Entities;
using Cross_WebApplication.Repository.Abstract;
using MongoDB.Driver;

namespace Cross_WebApplication.Repository.Concrete
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(MongoDbContext context) : base(context, "Roles") { }

        public async Task<Role> GetByName(string name)
        {
            var role = _collection.AsQueryable<Role>()
                .Where(role => role.Name == name).FirstOrDefault();
            return role;
        }
    }
}
