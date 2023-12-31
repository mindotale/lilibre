﻿using Lilibre.Application;

using Microsoft.EntityFrameworkCore;

namespace Lilibre.Persistence;

public class AuthorRepository : IRepository<Author, int>
{
    private readonly ApplicationDbContext _context;

    public AuthorRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Author?> GetByIdAsync(int id)
    {
        return await _context.Authors.FindAsync(id);
    }

    public async Task<IList<Author>> GetAllAsync(int offset, int limit)
    {
        return await _context.Authors.Skip(offset).Take(limit).ToListAsync();
    }

    public async Task<int> AddAsync(Author entity)
    {
        await _context.Authors.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity.Id;
    }

    public async Task UpdateAsync(Author entity)
    {
        _context.Authors.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.Authors.FindAsync(id);
        if (entity is null)
        {
            return false;
        }

        _context.Authors.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}