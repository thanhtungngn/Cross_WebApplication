using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Cross_WebApplication.Entities
{
    public class BaseEntity
    {
        public Guid Id { get; set; }
    }
}
