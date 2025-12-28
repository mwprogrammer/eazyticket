using EazyTicket.Core.Entities;

namespace EazyTicket.Core.Repositories
{
    public interface IBaseRepository<T>
        where T : BaseEntity
    {
        public Task<List<T>> GetAllAsync();
        public Task<T?> GetByIdAsync(long Id);
        public Task<T?> GetByPropertyAsync<KValue>(string property, KValue value);
        public Task<List<T>> GetAllWithinDateRangeAsync(DateTime from, DateTime to);
        public Task<List<T>> GetAllByPropertyAsync<KValue>(string property, KValue value);
        public Task<T> AddAsync(T entity);
        public Task<T?> AddWithDuplicateCheckAsync<KValue>(T entity, string property, KValue value);
        public Task AddAllAsync(List<T> entities);
        public Task<T> UpdateAsync(T entity);
        public Task UpdateAllAsync(List<T> entities);
    }
}
