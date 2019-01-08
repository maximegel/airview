using System;
using System.Threading;
using System.Threading.Tasks;
using AirView.Api.Core.Internal;

namespace AirView.Api.Core
{
    public static class PagedQueryableExtensions
    {
        public static IPagedQueryable<T> FirstPage<T>(this IPagedQueryable<T> queryable) =>
            new PagedQueryable<T>(queryable, queryable.Limit, 0);

        public static bool HasNextPage<T>(this IPagedQueryable<T> queryable, int totalCount) =>
            queryable.Offset + queryable.Limit <= totalCount;

        public static async Task<bool> HasNextPageAsync<T>(
            this IPagedQueryable<T> queryable, CancellationToken cancellationToken = default) =>
            HasNextPage(queryable, await queryable.TotalCountAsync(cancellationToken));

        public static bool HasPreviousPage<T>(this IPagedQueryable<T> queryable) =>
            queryable.Offset > 0;

        public static IPagedQueryable<T> LastPage<T>(this IPagedQueryable<T> queryable, int totalCount) =>
            new PagedQueryable<T>(queryable, queryable.Limit,
                (int) Math.Ceiling(totalCount / (double) queryable.Limit));

        public static async Task<IPagedQueryable<T>> LastPageAsync<T>(
            this IPagedQueryable<T> queryable, CancellationToken cancellationToken = default) =>
            LastPage(queryable, await queryable.TotalCountAsync(cancellationToken));

        public static IPagedQueryable<T> NextPage<T>(this IPagedQueryable<T> queryable, int totalCount)
        {
            if (!HasNextPage(queryable, totalCount))
                throw new InvalidOperationException(
                    $"There is no page of size {queryable.Limit} starting at the index {queryable.Offset + queryable.Limit}.");

            return new PagedQueryable<T>(queryable, queryable.Limit, queryable.Offset + queryable.Limit);
        }

        public static async Task<IPagedQueryable<T>> NextPageAsync<T>(
            this IPagedQueryable<T> queryable, CancellationToken cancellationToken = default) =>
            NextPage(queryable, await queryable.TotalCountAsync(cancellationToken));

        public static IPagedQueryable<T> PreviousPage<T>(this IPagedQueryable<T> queryable) =>
            new PagedQueryable<T>(queryable, queryable.Limit, queryable.Offset - queryable.Limit);
    }
}