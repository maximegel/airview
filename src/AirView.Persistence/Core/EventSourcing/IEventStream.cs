using System.Collections.Generic;

namespace AirView.Persistence.Core.EventSourcing
{
    public interface IEventStream<TEvent> :
        IAsyncEnumerable<TEvent>
    {
        void Append(TEvent @event);

        IAsyncEnumerable<TEvent> From(long index);
    }
}