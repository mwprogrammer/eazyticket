using EazyTicket.Core.Entities;
using EazyTicket.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EazyTicket.Infrastructure.Persistence.Repositories;

public class BaseRepository<T>(DataContext dataContext)
    where T : BaseEntity, IBaseRepository<T>
{
    private readonly DataContext _dataContext = dataContext;

    public async Task<T> AddAsync(T entity)
    {
        var createdEntity = await _dataContext.Set<T>().AddAsync(entity);
        await _dataContext.SaveChangesAsync();

        return createdEntity.Entity;
    }

    public async Task<T?> AddWithDuplicateCheckAsync<KValue>(
        T entity,
        string property,
        KValue value
    )
    {
        var duplicate = await GetByPropertyAsync(property, value);

        if (duplicate != null)
        {
            return null;
        }

        var createdEntity = await _dataContext.Set<T>().AddAsync(entity);
        await _dataContext.SaveChangesAsync();

        return createdEntity.Entity;
    }

    public async Task AddAllAsync(List<T> entities)
    {
        await _dataContext.Set<T>().AddRangeAsync(entities);
        await _dataContext.SaveChangesAsync();
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await _dataContext
            .Set<T>()
            .Where(x => x.DeletedAt == null)
            .OrderByDescending(x => x.Id)
            .ToListAsync();
    }

    public async Task<List<T>> GetAllByPropertyAsync<KValue>(string property, KValue value)
    {
        return await _dataContext
            .Set<T>()
            .Where(x => Equals(EF.Property<KValue>(x, property), value) && x.DeletedAt == null)
            .ToListAsync();
    }

    public async Task<List<T>> GetAllWithinDateRangeAsync(DateTime from, DateTime to)
    {
        return await _dataContext
            .Set<T>()
            .AsNoTracking()
            .Where(x => x.CreatedAt >= from && x.CreatedAt <= to && x.DeletedAt == null)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    public async Task<T?> GetByIdAsync(long Id)
    {
        return await _dataContext
            .Set<T>()
            .FirstOrDefaultAsync(x => x.Id == Id && x.DeletedAt == null);
    }

    public async Task<T?> GetByPropertyAsync<KValue>(string property, KValue value)
    {
        return await _dataContext
            .Set<T>()
            .FirstOrDefaultAsync(x =>
                Equals(EF.Property<KValue>(x, property), value) && x.DeletedAt == null
            );
    }

    public async Task<T> UpdateAsync(T entity)
    {
        var updatedEntity = _dataContext.Set<T>().Update(entity);
        await _dataContext.SaveChangesAsync();

        return updatedEntity.Entity;
    }

    public async Task UpdateAllAsync(List<T> entities)
    {
        _dataContext.Set<T>().UpdateRange(entities);
        await _dataContext.SaveChangesAsync();
    }
}
