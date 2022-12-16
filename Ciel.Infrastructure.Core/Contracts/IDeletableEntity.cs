namespace Ciel.Infrastructure.Core.Contracts;

public interface IDeletableEntity
{
    bool IsDeleted { get; set; }
}