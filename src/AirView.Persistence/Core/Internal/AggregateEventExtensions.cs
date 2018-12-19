using AirView.Domain.Core;

namespace AirView.Persistence.Core.Internal
{
    internal static class AggregateEventExtensions
    {
        public static string GetName(this IAggregateEvent @event, IAggregateRoot aggregate) =>
            @event.GetType().Name
                .Replace("Aggregate", aggregate.GetName())
                .Replace("Event", "");
    }
}