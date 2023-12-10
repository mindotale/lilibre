using Lilibre.Application;

using Microsoft.EntityFrameworkCore;

namespace Lilibre.Persistence;

public class PublisherRepository : IRepository<Publisher, int>
{
    private readonly ApplicationDbContext _context;

    public PublisherRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Publisher?> GetByIdAsync(int id)
    {
        return await _context.Publishers.FindAsync(id);
    }

    public async Task<IList<Publisher>> GetAllAsync(int offset, int limit)
    {
        return await _context.Publishers
            .Skip(offset)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<int> AddAsync(Publisher entity)
    {
        await _context.Publishers.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity.Id;
    }

    public async Task UpdateAsync(Publisher entity)
    {
        _context.Publishers.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.Publishers.FindAsync(id);
        if (entity is null)
        {
            return false;
        }

        _context.Publishers.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}
