using System.Collections.Generic;

namespace AirView.Persistence.Core.EventSourcing.Internal
{
    internal class EventStream<TEvent> :
        IEventStream<TEvent>
    {
        private readonly IEventReader<TEvent> _reader;
        private readonly int _sliceSize;
        private readonly ICollection<TEvent> _uncommitedEvents = new List<TEvent>();

        public EventStream(object id, IEventReader<TEvent> reader, int sliceSize)
        {
            Id = id;
            _reader = reader;
            _sliceSize = sliceSize;
        }

        public object Id { get; }

        public IEnumerable<TEvent> UncommitedEvents => _uncommitedEvents;

        public IAsyncEnumerator<TEvent> GetEnumerator() => GetEnumeratorAt(0);

        public void Append(TEvent @event) =>
            _uncommitedEvents.Add(@event);

        public IAsyncEnumerable<TEvent> From(long index) =>
            new AsyncEnumerable<TEvent>(() => GetEnumeratorAt(index));

        private IAsyncEnumerator<TEvent> GetEnumeratorAt(long index) =>
            new PagingAsyncEnumerator<TEvent>(
                (limit, offset, cancellationToken) => _reader.ReadAsync(Id, offset, limit, cancellationToken),
                _sliceSize, index);
    }
}