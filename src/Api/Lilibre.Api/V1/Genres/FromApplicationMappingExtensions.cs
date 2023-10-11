namespace Lilibre.Api.V1.Genres;

public static class FromApplicationMappingExtensions
{
    public static Genre ToGenre(this Application.Genre genre)
    {
        return new Genre(genre.Id, genre.Name, genre.Description);
    }
}
