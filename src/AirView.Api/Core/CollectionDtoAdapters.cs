using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AirView.Api.Core
{
    public static class CollectionDtoAdapters
    {
        public static CollectionDto<T> ToCollectionDto<T>(this IEnumerable<T> sequence, int totalCount) =>
            new CollectionDto<T>(sequence, totalCount);

        public static async Task<CollectionDto<T>> ToCollectionDtoAsync<T>(
            this IQueryable<T> queryable, int totalCout, CancellationToken cancellationToken = default) =>
            ToCollectionDto(await Task.Run(() => queryable.ToArray(), cancellationToken), totalCout);
    }
}