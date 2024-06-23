using Cross_WebApplication.Entity;

namespace Cross_WebApplication.Services
{
    public interface IJwtService
    {
        string GenerateToken(ApplicationUser user);
    }
}
