using Ciel.Infrastructure.Core.Contracts;
using Ciel.Infrastructure.Core.Contracts.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace Ciel.Infrastructure.Repositories;

public class Repository<TEntity> : QueryRepository<TEntity>, IRepository<TEntity>
    where TEntity : class, IEntity<int>
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<TEntity> _dbSet;
    public Repository(ApplicationDbContext context) : base(context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        await _dbSet.AddAsync(entity, cancellationToken).ConfigureAwait(false);
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task AddAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        if (entities == null)
        {
            throw new ArgumentNullException(nameof(entities));
        }

        await _dbSet.AddRangeAsync(entities, cancellationToken).ConfigureAwait(false);
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        _dbSet.Update(entity);
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task UpdateAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        if (entities == null)
        {
            throw new ArgumentNullException(nameof(entities));
        }

        _dbSet.UpdateRange(entities);
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public virtual async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        _dbSet.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public virtual async Task DeleteAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        if (entities == null)
        {
            throw new ArgumentNullException(nameof(entities));
        }

        _dbSet.RemoveRange(entities);
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public Task<int> ExecuteSqlCommandAsync(string sql, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(sql))
        {
            throw new ArgumentNullException(nameof(sql));
        }

        return _context.Database.ExecuteSqlRawAsync(sql, cancellationToken);
    }

    public Task<int> ExecuteSqlCommandAsync(string sql, IEnumerable<DbParameter> parameters, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(sql))
        {
            throw new ArgumentNullException(nameof(sql));
        }

        return _context.Database.ExecuteSqlRawAsync(sql, parameters, cancellationToken);
    }

    public Task<int> ExecuteSqlCommandAsync(string sql, IEnumerable<object> parameters, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(sql))
        {
            throw new ArgumentNullException(nameof(sql));
        }

        return _context.Database.ExecuteSqlRawAsync(sql, parameters, cancellationToken);
    }


}