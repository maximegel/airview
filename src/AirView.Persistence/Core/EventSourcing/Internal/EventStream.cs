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

        public IAsyncEnumerator<TEvent> GetEnumerator() =>
            new PagingAsyncEnumerator<TEvent>(
                (limit, offset, cancellationToken) => _reader.ReadAsync(Id, limit, offset, cancellationToken),
                _sliceSize);

        public void Append(TEvent @event) =>
            _uncommitedEvents.Add(@event);

        public object Id { get; }

        public IEnumerable<TEvent> UncommitedEvents => _uncommitedEvents;
    }    
}