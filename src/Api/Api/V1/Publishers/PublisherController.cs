using Lilibre.Application;
using Lilibre.Contracts.V1.Publishers;

using Microsoft.AspNetCore.Mvc;

using Publisher = Lilibre.Contracts.V1.Publishers.Publisher;

namespace Lilibre.Api.V1.Publishers;

[Route("api/v1/[controller]")]
[ApiController]
public class PublisherController : ControllerBase
{
    private readonly IRepository<Application.Publisher, int> _publisherRepository;

    public PublisherController(IRepository<Application.Publisher, int> publisherRepository)
    {
        _publisherRepository = publisherRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Publisher>>> GetPublishers(int offset = 0, int limit = 10)
    {
        var publishers = await _publisherRepository.GetAllAsync(offset, limit);
        var response = publishers.Select(p => p.ToPublisher());
        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Publisher>> GetPublisher(int id)
    {
        var publisher = await _publisherRepository.GetByIdAsync(id);
        if (publisher is null)
        {
            return NotFound();
        }

        var response = publisher.ToPublisher();
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<Publisher>> CreatePublisher(CreatePublisher request)
    {
        var publisher = new Application.Publisher
        {
            Id = 0,
            Name = request.Name,
            Description = request.Description,
            Website = request.Website
        };

        var publisherId = await _publisherRepository.AddAsync(publisher);
        var response = new Publisher(publisherId, publisher.Name, publisher.Description, publisher.Website);
        return CreatedAtAction(nameof(GetPublisher), new { id = publisherId }, response);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdatePublisher(int id, UpdatePublisher request)
    {
        var publisher = await _publisherRepository.GetByIdAsync(id);
        if (publisher is null)
        {
            return NotFound();
        }

        publisher.Name = request.Name;
        publisher.Description = request.Description;
        publisher.Website = request.Website;

        await _publisherRepository.UpdateAsync(publisher);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeletePublisher(int id)
    {
        var deleted = await _publisherRepository.DeleteAsync(id);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}

internal static class PublisherExtensions
{
    public static Publisher ToPublisher(this Application.Publisher publisher)
    {
        return new Publisher(publisher.Id, publisher.Name, publisher.Description, publisher.Website);
    }
}
