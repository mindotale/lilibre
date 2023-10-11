using Lilibre.Application;
using Microsoft.EntityFrameworkCore;

namespace Lilibre.Persistence;

public class BookRepository : IRepository<Book, int>
{
    private readonly ApplicationDbContext _context;

    public BookRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Book?> GetByIdAsync(int id)
    {
        return await _context.Books
            .Include(b => b.Authors)
            .Include(b => b.Genres)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<IList<Book>> GetAllAsync(int offset, int limit)
    {
        return await _context.Books
            .Include(b => b.Authors)
            .Include(b => b.Genres)
            .Skip(offset)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<int> AddAsync(Book entity)
    {
        await _context.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity.Id;
    }

    public async Task UpdateAsync(Book entity)
    {
        _context.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.Books.FindAsync(id);
        if (entity is null)
        {
            return false;
        }

        _context.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}