using Microsoft.AspNetCore.Mvc;

namespace Lilibre.Api.V1.Genres;

[Route("api/v1/[controller]")]
[ApiController]
public class GenresController : ControllerBase
{
    [HttpGet]
    public Task<ActionResult<IEnumerable<Genre>>> GetGenres(int offset = 0, int limit = 10)
    {
        throw new NotImplementedException();
    }

    [HttpGet("{id:int}")]
    public Task<ActionResult<Genre>> GetGenre(int id)
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    public Task<ActionResult<Genre>> CreateGenre(CreateGenre request)
    {
        throw new NotImplementedException();
    }

    [HttpPut("{id:int}")]
    public Task<ActionResult> UpdateGenre(int id, UpdateGenre request)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("{id:int}")]
    public Task<ActionResult> DeleteGenre(int id)
    {
        throw new NotImplementedException();
    }
}

public sealed record Genre(int Id, string Name, string Description);

public sealed record CreateGenre(string Name, string Description);

public sealed record UpdateGenre(string Name, string Description);
