using Cross_WebApplication.Entity;

namespace Cross_WebApplication.Services
{
    public interface IJwtService
    {
        Task<string> GenerateToken(ApplicationUser user);
    }
}
