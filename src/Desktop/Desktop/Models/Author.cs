namespace Desktop.Models;

public sealed class Author
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int BirthYear { get; set; }
}
