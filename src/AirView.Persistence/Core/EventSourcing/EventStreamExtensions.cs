using System.Collections.Generic;
using AirView.Domain.Core;

namespace AirView.Persistence.Core.EventSourcing
{
    public static class EventStreamExtensions
    {
        public static void AppendRange<TEvent>(this IEventStream<TEvent> stream, IEnumerable<TEvent> events)
        {
            foreach (var @event in events) stream.Append(@event);
        }

        public static IAsyncEnumerable<TEvent> From<TEvent>(this IEventStream<TEvent> stream, TEvent @event)
            where TEvent : IDomainEvent =>
            stream.From(@event.AggregateVersion);
    }
}