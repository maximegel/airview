using System.Linq;
using AirView.Api.Core.Internal;

namespace AirView.Api.Core
{
    public static class PagedQueryableAdapters
    {
        public static IPagedQueryable<T> Paginate<T>(this IQueryable<T> queryable, int limit, int offset = 0) =>
            new PagedQueryable<T>(queryable, limit, offset);
    }
}