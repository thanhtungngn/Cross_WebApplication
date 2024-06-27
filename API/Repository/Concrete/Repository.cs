using Cross_WebApplication.Context;
using Cross_WebApplication.Entities;
using Cross_WebApplication.Repository.Abstract;
using MongoDB.Driver;

namespace Cross_WebApplication.Repository.Concrete
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly IMongoCollection<T> _collection;

        public Repository(MongoDbContext context, string collectionName)
        {
            _collection = context.Database.GetCollection<T>(collectionName);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<T> GetByIdAsync(string id)
        {
            return await _collection.Find(Builders<T>.Filter.Eq("_id", new Guid(id))).FirstOrDefaultAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _collection.InsertOneAsync(entity);
        }

        public async Task UpdateAsync(T entity, UpdateDefinition<T> updateDefinition)
        {
            await _collection.UpdateOneAsync(Builders<T>.Filter.Eq("_id", entity.Id), updateDefinition);
        }

        public async Task DeleteAsync(string id)
        {
            await _collection.DeleteOneAsync(Builders<T>.Filter.Eq("_id", new Guid(id)));
        }
    }
}
