using Cross_WebApplication.Entities;

namespace Cross_WebApplication.Repository.Abstract
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByEmailAsync(string email);
    }
}
