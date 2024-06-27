using MongoDB.Driver;

namespace Cross_WebApplication.Repository.Abstract
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(string id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity, UpdateDefinition<T> updateDefinition);
        Task DeleteAsync(string id);
    }
}
