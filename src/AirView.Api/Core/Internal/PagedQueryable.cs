using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace AirView.Api.Core.Internal
{
    internal class PagedQueryable<T> :
        IPagedQueryable<T>
    {
        private readonly IQueryable<T> _queryable;
        private int? _totalCount;

        public PagedQueryable(IQueryable<T> queryable, int limit, int offset)
        {
            _queryable = queryable;
            Limit = limit;
            Offset = offset;
        }

        private IQueryable<T> Page => _queryable.Skip(Offset).Take(Limit);

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable) Page).GetEnumerator();

        public IEnumerator<T> GetEnumerator() => Page.GetEnumerator();

        public int Limit { get; }

        public int Offset { get; }

        public Task<int> TotalCountAsync(CancellationToken cancellationToken = default) =>
            _totalCount.HasValue
                ? Task.FromResult(_totalCount.Value)
                : Task.Run(() => (int) (_totalCount = _queryable.Count()), cancellationToken);

        public Type ElementType => Page.ElementType;

        public Expression Expression => Page.Expression;

        public IQueryProvider Provider => Page.Provider;
    }
}