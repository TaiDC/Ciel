using Microsoft.EntityFrameworkCore;

namespace Ciel.Infrastructure.Extensions;

public static class QueryableExtensions
{
    public static async Task<List<T>> ToPaginatedListAsync<T>(
            this IQueryable<T> source,
            int pageIndex,
            int pageSize,
            CancellationToken cancellationToken = default)
            where T : class
    {
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        if (pageIndex < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(pageIndex), "The value of pageIndex must be greater than 0.");
        }

        if (pageSize < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(pageSize), "The value of pageSize must be greater than 0.");
        }

        //long count = await source.LongCountAsync(cancellationToken);

        int skip = (pageIndex - 1) * pageSize;

        return await source.Skip(skip).Take(pageSize).ToListAsync(cancellationToken);
    }
}