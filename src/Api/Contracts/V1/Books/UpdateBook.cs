namespace Lilibre.Contracts.V1.Books;

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
