using Ciel.Infrastructure.Core.Contracts.Repositories;
using Ciel.Infrastructure.Core.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Ciel.Infrastructure.Repositories;

public class DeletableEntityRepository<TEntity> : Repository<TEntity>, IDeletableEntityRepository<TEntity>
    where TEntity : class, IEntity<int>, IDeletableEntity
{
    public DeletableEntityRepository(ApplicationDbContext context) : base(context)
    {
    }

    public Task<List<TEntity>> GetListWithDeletedAsync(CancellationToken cancellationToken = default)
        => GetQueryable().ToListAsync(cancellationToken);

    public Task<List<TEntity>> GetListWithDeletedAsync(Expression<Func<TEntity, bool>> condition, CancellationToken cancellationToken = default)
        => GetQueryable().Where(condition).ToListAsync(cancellationToken);

    public override Task<List<TEntity>> GetListAsync(CancellationToken cancellationToken = default)
        => GetQueryable().Where(i => !i.IsDeleted).ToListAsync(cancellationToken);

    public override Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> condition, CancellationToken cancellationToken = default)
    {
        var query = GetQueryable();
        query = query.Where(condition);
        query = query.Where(i => !i.IsDeleted);

        return query.ToListAsync(cancellationToken);
    }

    public Task HardDeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
        => base.DeleteAsync(entity, cancellationToken);

    public Task HardDeleteAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        => base.DeleteAsync(entities, cancellationToken);

    public async Task UndeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        entity = entity ?? throw new ArgumentNullException(nameof(entity));
        entity.IsDeleted = false;
        await this.UpdateAsync(entity, cancellationToken);
    }

    public async Task UndeleteAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        entities = entities ?? throw new ArgumentNullException(nameof(entities));

        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
        }
        await this.UpdateAsync(entities, cancellationToken);
    }

    public override async Task DeleteAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        entities = entities ?? throw new ArgumentNullException(nameof(entities));

        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
        }
        await this.UpdateAsync(entities, cancellationToken);
    }

    public override async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        entity = entity ?? throw new ArgumentNullException(nameof(entity));

        entity.IsDeleted = true;
        await this.UpdateAsync(entity, cancellationToken);
    }
}