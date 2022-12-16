using System.Data.Common;
using System.Linq.Expressions;

namespace Ciel.Infrastructure.Core.Contracts.Repositories;

public interface IQueryRepository<TEntity>
    where TEntity : class, IEntity<int>
{
    Task<List<TEntity>> GetListAsync(CancellationToken cancellationToken = default);
    Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> condition, CancellationToken cancellationToken = default);
    Task<TEntity> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> condition, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> condition, CancellationToken cancellationToken = default);
    Task<bool> ExistsByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<int> GetCountAsync(CancellationToken cancellationToken = default);
    Task<int> GetCountAsync(Expression<Func<TEntity, bool>> condition, CancellationToken cancellationToken = default);
    Task<long> GetLongCountAsync(CancellationToken cancellationToken = default);
    Task<long> GetLongCountAsync(Expression<Func<TEntity, bool>> condition, CancellationToken cancellationToken = default);
    Task<List<TEntity>> GetFromRawSqlAsync(string sql, CancellationToken cancellationToken = default);
    Task<List<TEntity>> GetFromRawSqlAsync(string sql, object parameter, CancellationToken cancellationToken = default);
    Task<List<TEntity>> GetFromRawSqlAsync(string sql, IEnumerable<DbParameter> parameters, CancellationToken cancellationToken = default);
    Task<List<TEntity>> GetFromRawSqlAsync(string sql, IEnumerable<object> parameters, CancellationToken cancellationToken = default);
}