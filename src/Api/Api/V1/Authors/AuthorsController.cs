using Lilibre.Application;
using Lilibre.Contracts.V1.Authors;

using Microsoft.AspNetCore.Mvc;

using Author = Lilibre.Contracts.V1.Authors.Author;

namespace Lilibre.Api.V1.Authors;

[Route("api/v1/[controller]")]
[ApiController]
public class AuthorsController : ControllerBase
{
    private readonly IRepository<Application.Author, int> _authorRepository;

    public AuthorsController(IRepository<Application.Author, int> authorRepository)
    {
        _authorRepository = authorRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Author>>> GetAuthors(int offset = 0, int limit = 10)
    {
        var authors = await _authorRepository.GetAllAsync(offset, limit);
        var response = authors.Select(a => a.ToAuthor());
        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Author>> GetAuthor(int id)
    {
        var author = await _authorRepository.GetByIdAsync(id);
        if (author is null)
        {
            return NotFound();
        }

        var response = author.ToAuthor();
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<Author>> CreateAuthor(CreateAuthor request)
    {
        var author = new Application.Author
        {
            Id = 0,
            Name = request.Name,
            Description = request.Description,
            BirthYear = request.BirthYear
        };

        var authorId = await _authorRepository.AddAsync(author);
        var response = new Author(authorId, author.Name, author.Description, author.BirthYear);
        return CreatedAtAction(nameof(GetAuthor), new { id = authorId }, response);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateAuthor(int id, UpdateAuthor request)
    {
        var author = await _authorRepository.GetByIdAsync(id);
        if (author is null)
        {
            return NotFound();
        }

        author.Name = request.Name;
        author.Description = request.Description;
        author.BirthYear = request.BirthYear;
        await _authorRepository.UpdateAsync(author);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteAuthor(int id)
    {
        var deleted = await _authorRepository.DeleteAsync(id);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}

public static class FromApplicationMappingExtensions
{
    public static Author ToAuthor(this Application.Author author)
    {
        return new Author(author.Id, author.Name, author.Description, author.BirthYear);
    }
}
