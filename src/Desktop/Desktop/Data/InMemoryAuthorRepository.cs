using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

using Desktop.Models;

namespace Desktop.Data;

public sealed class InMemoryAuthorRepository : IAuthorRepository
{
    private readonly ConcurrentDictionary<int, Author> _authors = new();

    public InMemoryAuthorRepository()
    {
        Seed();
    }

    public IEnumerable<Author> GetAll()
    {
        return _authors.Values;
    }

    public Author? GetById(int id)
    {
        _authors.TryGetValue(id, out var author);
        return author;
    }

    public Author Add(Author author)
    {
        var id = _authors.Max(x => x.Key) + 1;
        author.Id = id;
        _authors[id] = author;
        return author;
    }

    public Author Update(Author author)
    {
        _authors[author.Id] = author;
        return author;
    }

    public void Remove(int id)
    {
        _authors.TryRemove(id, out _);
    }

    private void Seed()
    {
        var authors = new List<Author>
        {
            new()
            {
                Id = 1,
                Name = "Jane Austen",
                BirthYear = 1775,
                Description =
                    "English novelist known primarily for her six major novels, which interpret, critique and comment upon the British landed gentry at the end of the 18th century."
            },
            new()
            {
                Id = 2,
                Name = "Charles Dickens",
                BirthYear = 1812,
                Description =
                    "English writer and social critic. He created some of the world's best-known fictional characters and is regarded by many as the greatest novelist of the Victorian era."
            },
            new()
            {
                Id = 3,
                Name = "William Shakespeare",
                BirthYear = 1564,
                Description =
                    "English playwright, poet, and actor, widely regarded as the greatest writer in the English language and the world's greatest dramatist."
            }
        };

        foreach (var author in authors)
        {
            _authors[author.Id] = author;
        }
    }
}
