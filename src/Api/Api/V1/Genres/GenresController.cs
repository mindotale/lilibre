using Lilibre.Application;

using Microsoft.AspNetCore.Mvc;

namespace Lilibre.Api.V1.Genres;

[Route("api/v1/[controller]")]
[ApiController]
public class GenresController : ControllerBase
{
    private readonly IRepository<Application.Genre, int> _genreRepository;

    public GenresController(IRepository<Application.Genre, int> genreRepository)
    {
        _genreRepository = genreRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Genre>>> GetGenres(int offset = 0, int limit = 10)
    {
        var genres = await _genreRepository.GetAllAsync(offset, limit);
        var response = genres.Select(g => g.ToGenre());
        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Genre>> GetGenre(int id)
    {
        var genre = await _genreRepository.GetByIdAsync(id);
        if (genre is null)
        {
            return NotFound();
        }

        var response = genre.ToGenre();
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<Genre>> CreateGenre(CreateGenre request)
    {
        var genre = new Application.Genre
        {
            Id = 0,
            Name = request.Name,
            Description = request.Description
        };

        var genreId = await _genreRepository.AddAsync(genre);
        var response = new Genre(genreId, genre.Name, genre.Description);
        return CreatedAtAction(nameof(GetGenre), new { id = genreId }, response);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateGenre(int id, UpdateGenre request)
    {
        var genre = await _genreRepository.GetByIdAsync(id);
        if (genre is null)
        {
            return NotFound();
        }

        genre.Name = request.Name;
        genre.Description = request.Description;
        await _genreRepository.UpdateAsync(genre);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteGenre(int id)
    {
        var deleted = await _genreRepository.DeleteAsync(id);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}

public sealed record Genre(int Id, string Name, string Description);

public sealed record CreateGenre(string Name, string Description);

public sealed record UpdateGenre(string Name, string Description);

public static class FromApplicationMappingExtensions
{
    public static Genre ToGenre(this Application.Genre genre)
    {
        return new Genre(genre.Id, genre.Name, genre.Description);
    }
}
