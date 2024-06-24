using Cross_WebApplication.Entities;

namespace Cross_WebApplication.Repository.Abstract
{
    public interface IRoleRepository : IRepository<Role>
    {
        Task<Role> GetByName(string name);
    }
}
