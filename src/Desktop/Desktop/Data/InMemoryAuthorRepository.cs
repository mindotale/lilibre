using Desktop.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Desktop.Data;

public sealed class InMemoryAuthorRepository : IAuthorRepository
{
    private readonly ConcurrentDictionary<int, Author> _authors = new();

    public Task<IEnumerable<Author>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<Author>>(_authors.Values);
    }

    public Task<Author?> GetAsync(int id)
    {
        var author = _authors.GetValueOrDefault(id);
        return Task.FromResult<Author?>(author);
    }

    public Task<Author> CreateAsync(Author author)
    {
        var id = _authors.Count + 1;
        author.Id = id;
        _authors.TryAdd(id, author);
        return Task.FromResult(author);
    }

    public Task<Author> UpdateAsync(Author author)
    {
        _authors.TryUpdate(author.Id, author, author);
        return Task.FromResult(author);
    }

    public Task DeleteAsync(int id)
    {
        _authors.TryRemove(id, out _);
        return Task.CompletedTask;
    }
}
