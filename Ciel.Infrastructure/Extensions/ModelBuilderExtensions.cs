using Ciel.Infrastructure.Core.Contracts;
using Ciel.Infrastructure.Core.Contracts.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Ciel.Infrastructure.Extensions;

public static class ModelBuilderExtensions
{
    public static ModelBuilder ApplyIdentityEntityConfiguration(this ModelBuilder modelBuilder)
    {
        if (modelBuilder == null)
        {
            throw new ArgumentNullException(nameof(modelBuilder));
        }

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var tableName = entityType.GetTableName();
            if (tableName?.StartsWith("AspNet") ?? false)
            {
                entityType.SetTableName(tableName[6..]);
            }
        }

        return modelBuilder;
    }

    public static ModelBuilder EntityIndexesConfiguration(this ModelBuilder modelBuilder)
    {
        // IDeletableEntity.IsDeleted index
        if (modelBuilder == null)
        {
            throw new ArgumentNullException(nameof(modelBuilder));
        }

        var deletableEntityTypes = modelBuilder.Model
            .GetEntityTypes()
            .Where(et => et.ClrType != null && typeof(IDeletableEntity).IsAssignableFrom(et.ClrType));

        foreach (var deletableEntityType in deletableEntityTypes)
        {
            modelBuilder.Entity(deletableEntityType.ClrType).HasIndex(nameof(IDeletableEntity.IsDeleted));
        }

        return modelBuilder;
    }

    public static void ApplyAuditInfoOnSaveChanges(this ChangeTracker changeTracker, IClaimService claimService)
    {
        if (changeTracker == null)
        {
            throw new ArgumentNullException(nameof(changeTracker));
        }

        var entries = changeTracker.Entries<IAuditInfo>()
                                   .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);
        if (entries.Any())
        {
            foreach (var entityEntry in entries)
            {
                entityEntry.Entity.UpdatedBy = claimService.GetUserId();
                entityEntry.Entity.UpdatedOn = DateTime.Now;
                if (entityEntry.State == EntityState.Added)
                {
                    entityEntry.Entity.CreatedBy = claimService.GetUserId();
                    entityEntry.Entity.CreatedOn = DateTime.Now;
                }
            }
        }
    }
}