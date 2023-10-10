namespace Lilibre.Application;

public interface IRepository<TEntity, TId> where TEntity : IEntity<TId> where TId : IEquatable<TId>
{
    Task<TEntity?> GetByIdAsync(TId id);

    Task<IList<TEntity>> GetAllAsync(int offset, int limit);

    Task<TId> AddAsync(TEntity entity);

    Task UpdateAsync(TEntity entity);

    Task<bool> DeleteAsync(TId id);
}
