using System.Data.Common;

namespace Ciel.Infrastructure.Core.Contracts.Repositories;

public interface IRepository<TEntity> : IQueryRepository<TEntity>
    where TEntity : class, IEntity<int>
{
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task AddAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task UpdateAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task DeleteAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    Task<int> ExecuteSqlCommandAsync(string sql, CancellationToken cancellationToken = default);

    Task<int> ExecuteSqlCommandAsync(string sql, IEnumerable<DbParameter> parameters, CancellationToken cancellationToken = default);

    Task<int> ExecuteSqlCommandAsync(string sql, IEnumerable<object> parameters, CancellationToken cancellationToken = default);
}