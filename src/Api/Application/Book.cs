namespace Lilibre.Application;

public sealed class Book : Entity<int>
{
    public string Title { get; set; } = string.Empty;

    public List<Author> Authors { get; set; } = new();

    public List<Genre> Genres { get; set; } = new();

    public string Description { get; set; } = string.Empty;

    public string Isbn { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public int Pages { get; set; }

    public int Year { get; set; }

    public  Publisher Publisher { get; set; } = new();
}
