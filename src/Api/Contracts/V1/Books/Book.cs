using Lilibre.Contracts.V1.Authors;
using Lilibre.Contracts.V1.Genres;
using Lilibre.Contracts.V1.Publishers;

namespace Lilibre.Contracts.V1.Books;

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
