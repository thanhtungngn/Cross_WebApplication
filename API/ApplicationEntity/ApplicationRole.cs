using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace Cross_WebApplication.Entity
{
    [CollectionName("IdentityRoles")]
    public class ApplicationRole : MongoIdentityRole<Guid>
    {
        public ApplicationRole() :base() { }
        public ApplicationRole(string roleName) : base(roleName) { }
    }
}
