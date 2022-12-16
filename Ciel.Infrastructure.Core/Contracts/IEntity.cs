namespace Ciel.Infrastructure.Core.Contracts;

public interface IEntity<TKey>
{
    TKey Id { get; }
}