using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AirView.Domain.Core;

namespace AirView.Persistence.Core.Internal
{
    internal static class AggregateRootExtensions
    {
        public static Task PublishEventsAsync(
            this IAggregateRoot aggregate, IEventPublisher eventPublisher,
            CancellationToken cancellationToken = default) =>
            Task.WhenAll(aggregate.UncommittedEvents.Select(@event =>
                    eventPublisher.PublishAsync(@event, cancellationToken)))
                .ContinueWith(_ => aggregate.ClearUncommitedEvents());
    }
}