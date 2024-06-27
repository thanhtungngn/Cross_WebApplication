using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace Cross_WebApplication.Entity
{
    [CollectionName("IdentityUsers")]
    public class ApplicationUser : MongoIdentityUser<Guid>
    {
    }
}
