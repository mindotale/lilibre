using Lilibre.Application;

using Microsoft.EntityFrameworkCore;

namespace Lilibre.Persistence;

public class GenreRepository : IRepository<Genre, int>
{
    private readonly ApplicationDbContext _context;

    public GenreRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Genre?> GetByIdAsync(int id)
    {
        return await _context.FindAsync<Genre>(id);
    }

    public async Task<IList<Genre>> GetAllAsync(int offset, int limit)
    {
        return await _context.Set<Genre>().Skip(offset).Take(limit).ToListAsync();
    }

    public async Task<int> AddAsync(Genre entity)
    {
        await _context.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity.Id;
    }

    public async Task UpdateAsync(Genre entity)
    {
        _context.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.FindAsync<Genre>(id);
        if (entity is null)
        {
            return false;
        }

        _context.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}