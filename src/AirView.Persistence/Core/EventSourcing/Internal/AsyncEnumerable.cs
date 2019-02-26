using System;
using System.Collections.Generic;

namespace AirView.Persistence.Core.EventSourcing.Internal
{
    internal class AsyncEnumerable<T> :
        IAsyncEnumerable<T>
    {
        private readonly Func<IAsyncEnumerator<T>> _enumeratorFactory;

        public AsyncEnumerable(Func<IAsyncEnumerator<T>> enumeratorFactory) =>
            _enumeratorFactory = enumeratorFactory;

        public IAsyncEnumerator<T> GetEnumerator() => _enumeratorFactory();
    }
}