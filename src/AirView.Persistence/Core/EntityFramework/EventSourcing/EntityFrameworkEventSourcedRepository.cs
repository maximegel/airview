using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AirView.Domain.Core;
using AirView.Domain.Core.Internal;
using AirView.Persistence.Core.Internal;
using AirView.Shared.Railways;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AirView.Persistence.Core.EntityFramework.EventSourcing
{
    // TODO(maximegelinas): Move storage engine specific logic under a 'IEventLog' interface.
    public class EntityFrameworkEventSourcedRepository<TId, TAggregate, TDbContext> :
        IWritableRepository<TId, TAggregate>
        where TAggregate : IAggregateRoot<TId>
        where TDbContext : EventSourcedDbContext
    {
        private readonly Func<TId, TAggregate> _aggregateFactory = AggregateFactory.CreateByReflexion<TAggregate, TId>;
        private readonly TDbContext _context;
        private readonly IEventPublisher _eventPublisher;
        private readonly IDictionary<TId, TAggregate> _trakedAggregates = new Dictionary<TId, TAggregate>();

        public EntityFrameworkEventSourcedRepository(TDbContext context, IEventPublisher eventPublisher)
        {
            _context = context;
            _eventPublisher = eventPublisher;
        }

        public void Add(TAggregate aggregate) =>
            Attach(aggregate);

        public void Attach(TAggregate aggregate) =>
            _trakedAggregates[aggregate.Id] = aggregate;

        public void Remove(TAggregate aggregate)
        {
            Attach(aggregate);
            aggregate.RaiseEvent(AggregateRemovedEvent.Of(aggregate));
        }

        public async Task SaveAsync(CancellationToken cancellationToken)
        {
            _context.Events.AddRange(
                _trakedAggregates
                    .Values
                    .SelectMany(aggregate => aggregate.UncommittedEvents.Select(@event => new PersistentEvent
                    {
                        StreamId = aggregate.Id.ToString(),
                        StreamVersion = @event.AggregateVersion,
                        Name = @event.Data.GetName(aggregate),
                        Timestamp = DateTimeOffset.UtcNow,
                        Metadata = JsonConvert.SerializeObject(new {DataType = @event.Data.GetType().Name}),
                        Data = JsonConvert.SerializeObject(@event.Data)
                    })));

            await _context.SaveChangesAsync(cancellationToken);
            await Task.WhenAll(_trakedAggregates.Values.Select(aggregate =>
                aggregate.PublishEventsAsync(_eventPublisher, cancellationToken)));
            _trakedAggregates.Clear();
        }

        // TODO(maximegelinas): Use batch requests to avoid loading every events of a stream in memory at once.
        public async Task<Option<TAggregate>> TryFindAsync(TId id, CancellationToken cancellationToken)
        {
            var persistentEvents = await _context.Events
                .Where(@event => @event.StreamId == id.ToString())
                .OrderBy(@event => @event.StreamVersion)
                .ToArrayAsync(cancellationToken);

            return persistentEvents
                .Flatten(@event =>
                    typeof(TAggregate).Assembly.GetTypes()
                        .Where(type =>
                            type.Name == (string) JsonConvert.DeserializeObject<dynamic>(@event.Metadata).DataType)
                        .TrySingle(type => type.GetInterfaces().Any(@interface =>
                            @interface == typeof(IAggregateEvent)))
                        .Map(dataType => dataType.IsGenericType && dataType.GetGenericArguments().Length == 2
                            ? dataType.MakeGenericType(typeof(TAggregate), typeof(TId))
                            : dataType)
                        .Map(dataType => DomainEvent.Of<TAggregate>(
                            (TId) (Guid.Parse(@event.StreamId) as object),
                            @event.StreamVersion,
                            dataType, JsonConvert.DeserializeObject(@event.Data, dataType))))
                .DefaultIfEmpty(DomainEvent.Of<TAggregate>(id, 0, AggregateNeverCreatedEvent.Instance))
                .Aggregate(Option.Some(_aggregateFactory(id)), (aggregate, @event) => @event.ApplyTo(aggregate))
                .Do(Attach);
        }
    }
}