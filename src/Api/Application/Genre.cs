namespace Lilibre.Application;

public class Genre : Entity<int>
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
}
