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
        return await _context.Genres.FindAsync(id);
    }

    public async Task<IList<Genre>> GetAllAsync(int offset, int limit)
    {
        return await _context.Genres.Skip(offset).Take(limit).ToListAsync();
    }

    public async Task<int> AddAsync(Genre entity)
    {
        await _context.Genres.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity.Id;
    }

    public async Task UpdateAsync(Genre entity)
    {
        _context.Genres.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.Genres.FindAsync(id);
        if (entity is null)
        {
            return false;
        }

        _context.Genres.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}