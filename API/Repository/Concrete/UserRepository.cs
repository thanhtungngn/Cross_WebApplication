using Cross_WebApplication.Context;
using Cross_WebApplication.Entities;
using Cross_WebApplication.Repository.Abstract;
using MongoDB.Driver;

namespace Cross_WebApplication.Repository.Concrete
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(MongoDbContext context) : base(context, "Users") { }

        public async Task<User> GetByEmailAsync(string email)
        {
            var user = _collection.AsQueryable<User>()
                .Where(role => role.Email == email).FirstOrDefault();
            return user;
        }
    }
}
