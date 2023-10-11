namespace Lilibre.Api.V1.Authors;

public static class FromApplicationMappingExtensions
{
    public static Author ToAuthor(this Application.Author author)
    {
        return new Author(author.Id, author.Name, author.Description, author.BirthYear);
    }
}
