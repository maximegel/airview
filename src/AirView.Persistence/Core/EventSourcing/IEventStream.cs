using System.Collections.Generic;

namespace AirView.Persistence.Core.EventSourcing
{
    public interface IEventStream<TEvent> :
        IAsyncEnumerable<TEvent>
    {
        object Id { get; }

        IEnumerable<TEvent> UncommitedEvents { get; }

        void Append(TEvent @event);
    }
}