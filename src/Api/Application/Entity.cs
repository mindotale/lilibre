namespace Lilibre.Application;

public interface IEntity<TId> where TId : IEquatable<TId>
{
    TId Id { get; set; }
}

public abstract class Entity<TId> : IEntity<TId> where TId : IEquatable<TId>
{
    public TId Id { get; set; } = default!;
}
