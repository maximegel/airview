using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AirView.Persistence.Core.EventSourcing.Internal
{
    public class PagingAsyncEnumerator<T> :
        IAsyncEnumerator<T>
    {
        private static readonly Func<int, long, CancellationToken, Task<IEnumerable<T>>> NoopReader =
            (limit, offset, cancellationToken) => Task.FromResult(Enumerable.Empty<T>());

        private readonly int _limit;
        private IEnumerator<T> _currentIterator;
        private long _offset;
        private Func<int, long, CancellationToken, Task<IEnumerable<T>>> _reader;

        public PagingAsyncEnumerator(Func<int, long, CancellationToken, Task<IEnumerable<T>>> reader, int limit)
        {
            _reader = reader;
            _limit = limit;
        }

        public T Current { get; private set; }

        public async Task<bool> MoveNext(CancellationToken cancellationToken)
        {
            // If there is still at least one element to read in the last readed page, \
            if (_currentIterator?.MoveNext() == true)
            {
                // we read this element without making any async calls.
                Current = _currentIterator.Current;
                return true;
            }

            var page = (await _reader(_limit, _offset, cancellationToken)).ToList();

            // If we hit the noop player or if the previous page was the last and exactly the desired size, \
            if (page.Count == 0)
                // there is no more elements.
                return false;

            // If its the last page, \
            if (page.Count < _limit)
                // we disable the next calls to the reader.
                _reader = NoopReader;

            _currentIterator = page.GetEnumerator();
            _offset = _offset + _limit + 1;

            if (!_currentIterator.MoveNext()) return false;
            Current = _currentIterator.Current;
            return true;
        }

        public void Dispose()
        {
        }
    }
}