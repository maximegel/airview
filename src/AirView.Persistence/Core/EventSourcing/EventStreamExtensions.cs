using System.Collections.Generic;

namespace AirView.Persistence.Core.EventSourcing
{
    public static class EventStreamExtensions
    {
        public static void AppendRange<TEvent>(this IEventStream<TEvent> stream, IEnumerable<TEvent> events)
        {
            foreach (var @event in events) stream.Append(@event);
        }
    }
}