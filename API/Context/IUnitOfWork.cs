using Cross_WebApplication.Repository.Abstract;

namespace Cross_WebApplication.Context
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        IRoleRepository Roles { get; }
        IEventRepository Events { get; }
    }
}
