using Ciel.Infrastructure.Core.Contracts;
using Ciel.Infrastructure.Core.Contracts.Repositories;
using Ciel.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Linq.Expressions;

namespace Ciel.Infrastructure.Repositories;

public class QueryRepository<TEntity> : IQueryRepository<TEntity>
    where TEntity : class, IEntity<int>
{
    private readonly ApplicationDbContext _context;
    public QueryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public virtual Task<List<TEntity>> GetListAsync(CancellationToken cancellationToken = default)
        => GetListAsync(null, cancellationToken);

    public virtual Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> condition, CancellationToken cancellationToken = default)
    {
        var query = GetQueryable();

        if (condition != null)
        {
            query = query.Where(condition);
        }

        return query.ToListAsync(cancellationToken);
    }

    public Task<TEntity> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        if (id == null)
        {
            throw new ArgumentNullException(nameof(id));
        }

        var query = GetQueryable();

        return query.FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
    }

    public Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> condition, CancellationToken cancellationToken = default)
    {
        var query = GetQueryable();

        return query.FirstOrDefaultAsync(condition, cancellationToken);
    }

    public Task<bool> ExistsAsync(CancellationToken cancellationToken = default)
        => ExistsAsync(null, cancellationToken);

    public Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> condition, CancellationToken cancellationToken = default)
    {
        var query = GetQueryable();

        if (condition != null)
        {
            query = query.Where(condition);
        }

        return query.AnyAsync(cancellationToken);
    }

    public Task<bool> ExistsByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        if (id == null)
        {
            throw new ArgumentNullException(nameof(id));
        }

        var query = GetQueryable();

        return query.AnyAsync(i => i.Id == id, cancellationToken);
    }

    public Task<int> GetCountAsync(CancellationToken cancellationToken = default)
        => GetCountAsync(null, cancellationToken);

    public Task<int> GetCountAsync(Expression<Func<TEntity, bool>> condition, CancellationToken cancellationToken = default)
    {
        var query = GetQueryable();

        if (condition != null)
        {
            query = query.Where(condition);
        }

        return query.CountAsync(cancellationToken);
    }

    public Task<long> GetLongCountAsync(CancellationToken cancellationToken = default)
        => GetLongCountAsync(null, cancellationToken);

    public Task<long> GetLongCountAsync(Expression<Func<TEntity, bool>> condition, CancellationToken cancellationToken = default)
    {
        var query = GetQueryable();

        if (condition != null)
        {
            query = query.Where(condition);
        }

        return query.LongCountAsync(cancellationToken);
    }

    public Task<List<TEntity>> GetFromRawSqlAsync(string sql, CancellationToken cancellationToken = default)
    {
        var parameters = new List<object>();

        return _context.GetFromQueryAsync<TEntity>(sql, parameters, cancellationToken);
    }

    public Task<List<TEntity>> GetFromRawSqlAsync(string sql, object parameter, CancellationToken cancellationToken = default)
    {
        var parameters = new List<object>() { parameter };

        return _context.GetFromQueryAsync<TEntity>(sql, parameters, cancellationToken);
    }

    public Task<List<TEntity>> GetFromRawSqlAsync(string sql, IEnumerable<DbParameter> parameters, CancellationToken cancellationToken = default)
    {
        return _context.GetFromQueryAsync<TEntity>(sql, parameters, cancellationToken);
    }

    public Task<List<TEntity>> GetFromRawSqlAsync(string sql, IEnumerable<object> parameters, CancellationToken cancellationToken = default)
    {
        return _context.GetFromQueryAsync<TEntity>(sql, parameters, cancellationToken);
    }

    public IQueryable<TEntity> GetQueryable()
    {
        return _context.Set<TEntity>();
    }
}