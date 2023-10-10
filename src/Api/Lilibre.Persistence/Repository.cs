using Lilibre.Application;

using Microsoft.EntityFrameworkCore;

namespace Lilibre.Persistence;

public class Repository<TEntity, TId> : IRepository<TEntity, TId>
    where TEntity : class, IEntity<TId> where TId : IEquatable<TId>
{
    private readonly ApplicationDbContext _context;

    public Repository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TEntity?> GetByIdAsync(TId id)
    {
        return await _context.FindAsync<TEntity>(id);
    }

    public async Task<IList<TEntity>> GetAllAsync(int offset, int limit)
    {
        return await _context.Set<TEntity>().Skip(offset).Take(limit).ToListAsync();
    }

    public async Task<TId> AddAsync(TEntity entity)
    {
        await _context.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity.Id;
    }

    public async Task UpdateAsync(TEntity entity)
    {
        _context.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(TId id)
    {
        var entity = await _context.FindAsync<TEntity>(id);
        if (entity is null)
        {
            return false;
        }

        _context.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}