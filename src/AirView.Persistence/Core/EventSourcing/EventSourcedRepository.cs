using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AirView.Domain.Core;
using AirView.Persistence.Core.Internal;
using AirView.Shared.Railways;

namespace AirView.Persistence.Core.EventSourcing
{
    public class EventSourcedRepository<TAggregate> :
        IWritableRepository<TAggregate>,
        IUnitOfWorkParticipant
        where TAggregate : IAggregateRoot
    {
        private readonly IEventLog<IDomainEvent> _eventLog;
        // TODO(maximegelinas): Move out event publishing responsability from event sourced repository.
        private readonly IEventPublisher _eventPublisher;

        // TODO(maximegelinas): Make stream ID naming convention configurable.
        private readonly Func<Type, object, string> _streamIdNamingConvention =
            (_, aggregateId) => aggregateId.ToString();

        private readonly IDictionary<object, TAggregate> _trakedAggregates = new Dictionary<object, TAggregate>();

        public EventSourcedRepository(
            IEventLog<IDomainEvent> eventLog,
            IEventPublisher eventPublisher,
            IUnitOfWorkContext unitOfWorkContext)
        {
            _eventLog = eventLog;
            _eventPublisher = eventPublisher;
            unitOfWorkContext.Enlist(this);
        }

        public void Dispose()
        {
        }

        public void Commit()
        {
            _trakedAggregates.Clear();
            _eventLog.Commit();
        }

        public void Prepare() => _eventLog.Prepare();

        public void Rollback()
        {
            _trakedAggregates.Clear();
            _eventLog.Rollback();
        }

        public async Task SaveAsync(CancellationToken cancellationToken)
        {
            foreach (var aggregate in _trakedAggregates.Values)
                _eventLog.Stream(StreamId(aggregate.Id))
                    .AppendRange(aggregate.UncommittedEvents);

            await _eventLog.SaveAsync(cancellationToken);
            await Task.WhenAll(_trakedAggregates.Values.Select(aggregate =>
                aggregate.PublishEventsAsync(_eventPublisher, cancellationToken)));
        }

        public void Add(TAggregate aggregate) =>
            Attach(aggregate);

        public void Attach(TAggregate aggregate) =>
            _trakedAggregates[aggregate.Id] = aggregate;

        public void Remove(TAggregate aggregate)
        {
            Attach(aggregate);
            aggregate.RaiseEvent(new AggregateRemovedEvent());
        }

        public async Task<Option<TAggregate>> TryFindAsync(object id, CancellationToken cancellationToken) =>
            (await _eventLog.Stream(StreamId(id))
                .DefaultIfEmpty(DomainEvent.Of<TAggregate>(id, 0, new AggregateNeverCreatedEvent()))
                .Aggregate(Option.Some(AggregateRoot.New<TAggregate>(id)),
                    (aggregate, @event) => @event.ApplyTo(aggregate), cancellationToken))
            .Do(Attach);

        private object StreamId(object aggregateId) => _streamIdNamingConvention(typeof(TAggregate), aggregateId);
    }
}