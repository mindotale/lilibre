using Microsoft.AspNetCore.Mvc;

namespace Lilibre.Api.V1.Authors;

[Route("api/v1/[controller]")]
[ApiController]
public class AuthorsController : ControllerBase
{
    [HttpGet]
    public Task<ActionResult<IEnumerable<Author>>> GetAuthors(int offset = 0, int limit = 10)
    {
        throw new NotImplementedException();
    }

    [HttpGet("{id:int}")]
    public Task<ActionResult<Author>> GetAuthor(int id)
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    public Task<ActionResult<Author>> CreateAuthor(CreateAuthor request)
    {
        throw new NotImplementedException();
    }

    [HttpPut("{id:int}")]
    public Task<ActionResult> UpdateAuthor(int id, UpdateAuthor request)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("{id:int}")]
    public Task<ActionResult> DeleteAuthor(int id)
    {
        throw new NotImplementedException();
    }
}

public sealed record Author(int Id, string Name, string Description, int BirthYear);

public sealed record CreateAuthor(string Name, string Description, int BirthYear);

public sealed record UpdateAuthor(string Name, string Description, int BirthYear);
