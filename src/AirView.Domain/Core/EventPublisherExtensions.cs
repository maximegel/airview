using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AirView.Domain.Core
{
    public static class EventPublisherExtensions
    {
        public static Task PublishAsync(
            this IEventPublisher eventPublisher, IAggregateRoot aggregate,
            CancellationToken cancellationToken = default) =>
            Task.WhenAll(aggregate.UncommittedEvents.Select(@event =>
                    eventPublisher.PublishAsync(@event, cancellationToken)))
                .ContinueWith(_ => aggregate.ClearUncommitedEvents());
    }
}