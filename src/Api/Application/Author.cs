namespace Lilibre.Application;

public sealed class Author: Entity<int>
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int BirthYear { get; set; }
}
