using Lilibre.Api.V1.Authors;
using Lilibre.Api.V1.Genres;

namespace Lilibre.Api.V1.Books;

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
            book.Year);
    }
}
