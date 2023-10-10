using Lilibre.Api.V1.Authors;
using Lilibre.Api.V1.Genres;

using Microsoft.AspNetCore.Mvc;

namespace Lilibre.Api.V1.Books;

[Route("api/v1/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    [HttpGet]
    public Task<ActionResult<IEnumerable<Book>>> GetBooks(int offset = 0, int limit = 10)
    {
        throw new NotImplementedException();
    }

    [HttpGet("{id:int}")]
    public Task<ActionResult<Book>> GetBook(int id)
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    public Task<ActionResult<Book>> CreateBook(CreateBook request)
    {
        throw new NotImplementedException();
    }

    [HttpPut("{id:int}")]
    public Task<ActionResult> UpdateBook(int id, UpdateBook request)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("{id:int}")]
    public Task<ActionResult> DeleteBook(int id)
    {
        throw new NotImplementedException();
    }
}

public sealed record Book(
    int Id,
    string Title,
    IEnumerable<Author> Authors,
    IEnumerable<Genre> Genres,
    string Description,
    string Isbn,
    decimal Price,
    int Pages);

public sealed record CreateBook(
    string Title,
    IEnumerable<int> Authors,
    IEnumerable<int> Genres,
    string Description,
    string Isbn,
    decimal Price,
    int Pages);

public sealed record UpdateBook(
    string Title,
    IEnumerable<int> Authors,
    IEnumerable<int> Genres,
    string Description,
    string Isbn,
    decimal Price,
    int Pages);
