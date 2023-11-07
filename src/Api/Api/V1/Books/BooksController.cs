using Lilibre.Api.V1.Authors;
using Lilibre.Api.V1.Genres;
using Lilibre.Api.V1.Publishers;
using Lilibre.Application;

using Microsoft.AspNetCore.Mvc;

using Author = Lilibre.Api.V1.Authors.Author;
using Genre = Lilibre.Api.V1.Genres.Genre;
using Publisher = Lilibre.Api.V1.Publishers.Publisher;

namespace Lilibre.Api.V1.Books;

[Route("api/v1/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly IRepository<Application.Book, int> _bookRepository;
    private readonly IRepository<Application.Author, int> _authorRepository;
    private readonly IRepository<Application.Genre, int> _genreRepository;
    private readonly IRepository<Application.Publisher, int> _publisherRepository;

    public BooksController(IRepository<Application.Book, int> bookRepository, IRepository<Application.Author, int> authorRepository, IRepository<Application.Genre, int> genreRepository, IRepository<Application.Publisher, int> publisherRepository)
    {
        _bookRepository = bookRepository;
        _authorRepository = authorRepository;
        _genreRepository = genreRepository;
        _publisherRepository = publisherRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Book>>> GetBooks(int offset = 0, int limit = 10)
    {
        var books = await _bookRepository.GetAllAsync(offset, limit);
        var response = books.Select(b => b.ToBook());
        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Book>> GetBook(int id)
    {
        var book = await _bookRepository.GetByIdAsync(id);
        if (book is null)
        {
            return NotFound();
        }

        var response = book.ToBook();
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<Book>> CreateBook(CreateBook request)
    {
        var authors = new List<Application.Author>();
        foreach (var id in request.AuthorIds)
        {
            var author = await _authorRepository.GetByIdAsync(id);
            if (author is null)
            {
                return BadRequest();
            }

            authors.Add(author);
        }

        var genres = new List<Application.Genre>();
        foreach (var id in request.GenreIds)
        {
            var genre = await _genreRepository.GetByIdAsync(id);
            if (genre is null)
            {
                return BadRequest();
            }

            genres.Add(genre);
        }

        var publisher = await _publisherRepository.GetByIdAsync(request.PublisherId);
        if (publisher is null)
        {
            return BadRequest();
        }

        var book = new Application.Book
        {
            Id = 0,
            Title = request.Title,
            Authors = authors,
            Genres = genres,
            Description = request.Description,
            Isbn = request.Isbn,
            Price = request.Price,
            Pages = request.Pages,
            Year = request.Year,
            Publisher = publisher
        };

        var bookId = await _bookRepository.AddAsync(book);
        var response = book.ToBook();
        return CreatedAtAction(nameof(GetBook), new { id = bookId }, response);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateBook(int id, UpdateBook request)
    {
        var book = await _bookRepository.GetByIdAsync(id);
        if (book is null)
        {
            return NotFound();
        }

        var authors = new List<Application.Author>();
        foreach (var authorId in request.AuthorIds)
        {
            var author = await _authorRepository.GetByIdAsync(authorId);
            if (author is null)
            {
                return BadRequest();
            }

            authors.Add(author);
        }

        var genres = new List<Application.Genre>();
        foreach (var genreId in request.GenreIds)
        {
            var genre = await _genreRepository.GetByIdAsync(genreId);
            if (genre is null)
            {
                return BadRequest();
            }

            genres.Add(genre);
        }

        var publisher = await _publisherRepository.GetByIdAsync(request.PublisherId);
        if (publisher is null)
        {
            return BadRequest();
        }

        book.Title = request.Title;
        book.Authors = authors;
        book.Genres = genres;
        book.Description = request.Description;
        book.Isbn = request.Isbn;
        book.Price = request.Price;
        book.Pages = request.Pages;
        book.Year = request.Year;
        book.Publisher = publisher;
        await _bookRepository.UpdateAsync(book);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteBook(int id)
    {
        var deleted = await _bookRepository.DeleteAsync(id);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
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
    int Pages,
    int Year,
    Publisher Publisher);

public sealed record CreateBook(
    string Title,
    IEnumerable<int> AuthorIds,
    IEnumerable<int> GenreIds,
    string Description,
    string Isbn,
    decimal Price,
    int Pages,
    int Year,
    int PublisherId);

public sealed record UpdateBook(
    string Title,
    IEnumerable<int> AuthorIds,
    IEnumerable<int> GenreIds,
    string Description,
    string Isbn,
    decimal Price,
    int Pages,
    int Year,
    int PublisherId);

public static class FromApplicationMappingExtensions
{
    public static Book ToBook(this Application.Book book)
    {
        return new Book(
            book.Id,
            book.Title,
            book.Authors.Select(a => a.ToAuthor()),
            book.Genres.Select(g => g.ToGenre()),
            book.Description,
            book.Isbn,
            book.Price,
            book.Pages,
            book.Year,
            book.Publisher.ToPublisher());
    }
}