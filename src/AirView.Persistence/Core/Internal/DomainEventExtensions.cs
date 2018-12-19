using AirView.Domain.Core;
using AirView.Shared.Railways;

namespace AirView.Persistence.Core.Internal
{
    internal static class DomainEventExtensions
    {
        public static Option<TAggregate> ApplyTo<TAggregate>(this IDomainEvent @event, Option<TAggregate> aggregate)
            where TAggregate : IAggregateRoot
        {
            switch (@event.Data)
            {
                case AggregateNeverCreatedEvent _: return Option.None;
                case AggregateRemovedEvent _: return Option.None;
                default: return aggregate.Do(value => value.ApplyEvent(@event));
            }
        }
    }
}