using System.Linq.Expressions;

namespace Ciel.Infrastructure.Core.Contracts.Repositories;

public interface IDeletableEntityRepository<TEntity> : IRepository<TEntity>
    where TEntity : class, IEntity<int>,  IDeletableEntity
{
    Task HardDeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task HardDeleteAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
    Task UndeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task UndeleteAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    Task<List<TEntity>> GetListWithDeletedAsync(CancellationToken cancellationToken = default);
    Task<List<TEntity>> GetListWithDeletedAsync(Expression<Func<TEntity, bool>> condition, CancellationToken cancellationToken = default);
}