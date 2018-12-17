using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using AirView.Domain.Core;
using AirView.Domain.Core.Internal;
using AirView.Shared.Railways;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AirView.Persistence.Core.EntityFramework.EventSourcing
{
    public class EntityFrameworkEventSourcedRepository<TId, TAggregate, TDbContext> :
        IWritableRepository<TId, TAggregate>
        where TAggregate : IAggregateRoot<TAggregate, TId>
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
            throw new NotImplementedException();
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
                        Name = @event.Data.GetType().Name.Replace("Event", ""),
                        Timestamp = DateTimeOffset.UtcNow,
                        Metadata = JsonConvert.SerializeObject(new
                        {
                            DataType = @event.Data.GetType().AssemblyQualifiedName
                        }),
                        Data = JsonConvert.SerializeObject(@event.Data)
                    })));

            await _context.SaveChangesAsync(cancellationToken);
            await Task.WhenAll(_trakedAggregates.Values.Select(aggregate => 
                PublishEvents(aggregate, cancellationToken)));
            _trakedAggregates.Clear();
        }

        public async Task<Option<TAggregate>> TryFindAsync(TId id, CancellationToken cancellationToken)
        {
            var persistentEvents = await _context.Events
                .Where(@event => @event.StreamId == id.ToString())
                .OrderBy(@event => @event.StreamVersion)
                .ToArrayAsync(cancellationToken);

            var domainEvents = persistentEvents
                .Select(@event =>
                {
                    var metadata = JsonConvert.DeserializeObject<dynamic>(@event.Metadata);
                    var dataType = Type.GetType((string) metadata.DataType);
                    var evenType = typeof(DomainEvent<,,>).MakeGenericType(typeof(TAggregate), typeof(TId), dataType);

                    return
                        (IDomainEvent<TAggregate, TId>)
                        evenType.GetConstructor(
                                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
                                null, new[] {typeof(TId), typeof(long), dataType}, null)
                            ?.Invoke(new[]
                            {
                                (TId) (Guid.Parse(@event.StreamId) as object),
                                @event.StreamVersion,
                                JsonConvert.DeserializeObject(@event.Data, dataType)
                            })
                        ?? throw new InvalidOperationException($"Constructor not found for domain event '{evenType.Name}'.");
                })
                .ToList();

            if (!persistentEvents.Any()) return Option.None;

            var aggregate = _aggregateFactory(id);
            domainEvents.ForEach(@event => aggregate.ApplyEvent(@event));

            Attach(aggregate);
            return aggregate;
        }

        private Task PublishEvents(TAggregate aggregate, CancellationToken cancellationToken) =>
            Task.WhenAll(aggregate.UncommittedEvents.Select(@event =>
                    _eventPublisher.PublishAsync(@event, cancellationToken)))
                .ContinueWith(_ => aggregate.ClearUncommitedEvents(), cancellationToken);
    }
}