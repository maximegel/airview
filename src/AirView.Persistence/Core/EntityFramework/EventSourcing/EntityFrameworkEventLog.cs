using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AirView.Domain.Core;
using AirView.Persistence.Core.EventSourcing;
using AirView.Persistence.Core.EventSourcing.Internal;
using AirView.Persistence.Core.Internal;
using AirView.Shared;
using AirView.Shared.Railways;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;

namespace AirView.Persistence.Core.EntityFramework.EventSourcing
{
    // TODO(maximegelinas): Handle concurrency.
    public class EntityFrameworkEventLog<TAggregate> :
        IEventLog<IDomainEvent>,
        IEventReader<IDomainEvent>
        where TAggregate : IAggregateRoot
    {
        private readonly EventSourcedDbContext _dbContext;

        private readonly IDictionary<object, EventStream<IDomainEvent>> _trackedStreams =
            new Dictionary<object, EventStream<IDomainEvent>>();

        private IDbContextTransaction _transaction;

        public EntityFrameworkEventLog(EventSourcedDbContext dbContext) =>
            _dbContext = dbContext;

        public void Dispose()
        {
        }

        public IEventStream<IDomainEvent> Stream(object id) =>
            _trackedStreams.TryGetValue(id)
                // TODO(maximegelinas): Make slice size (i.e. 200) configurable.
                .Reduce(() => _trackedStreams[id] = new EventStream<IDomainEvent>(id, this, 200));

        async Task<IEnumerable<IDomainEvent>> IEventReader<IDomainEvent>.ReadAsync(
            object streamId, long startIndex, int limit, CancellationToken cancellationToken) =>
            (await _dbContext.Events.AsNoTracking()
                .Where(@event =>
                    @event.StreamId == streamId.ToString() &&
                    startIndex <= @event.StreamVersion && @event.StreamVersion <= startIndex + limit)
                .OrderBy(@event => @event.StreamVersion)
                .ToListAsync(cancellationToken))
            .Select(@event =>
            {
                var dataTypeName = (string) JsonConvert.DeserializeObject<dynamic>(@event.Metadata).DataType;
                return typeof(TAggregate).Assembly.GetTypes()
                    .Where(type => type.Name == dataTypeName)
                    .TrySingle(type => typeof(IAggregateEvent).IsAssignableFrom(type))
                    .Map(dataType => DomainEvent.Of<TAggregate>(
                        // TODO(maximegelinas): Allow any type of identifier.
                        Guid.Parse(@event.StreamId),
                        @event.StreamVersion,
                        dataType, JsonConvert.DeserializeObject(@event.Data, dataType)))
                    .Reduce(() => throw new Exception(
                        $"Failed to construct domain event '{@event.StreamId}@{@event.StreamVersion}' from persistent event of type '{dataTypeName}'. " +
                        $"A class named {dataTypeName} and implementing '{typeof(IAggregateEvent).GetFriendlyName()}' should be present in the assembly " +
                        $"'{typeof(TAggregate).Assembly.GetName().Name}'."));
            });

        public void Commit()
        {
            _trackedStreams.Clear();
            _transaction.Commit();
        }

        public void Prepare() =>
            _transaction = _dbContext.Database.CurrentTransaction ?? _dbContext.Database.BeginTransaction();

        public void Rollback()
        {
            _trackedStreams.Clear();
            _transaction.Rollback();
        }

        public async Task SaveAsync(CancellationToken cancellationToken) =>
            await Task.WhenAll(_trackedStreams.Values.Select(stream =>
                WriteAsync(stream.Id, stream.UncommitedEvents, cancellationToken)));

        private Task WriteAsync(object streamId, IEnumerable<IDomainEvent> events, CancellationToken cancellationToken)
        {
            _dbContext.Events.AddRange(events.Select(@event => new PersistentEvent
            {
                StreamId = streamId.ToString(),
                StreamVersion = @event.AggregateVersion,
                Name = @event.GetName(),
                // TODO(maximegelinas): Use 'IClock' interface.
                Timestamp = DateTimeOffset.UtcNow,
                Metadata = JsonConvert.SerializeObject(new {DataType = @event.Data.GetType().Name}),
                Data = JsonConvert.SerializeObject(@event.Data)
            }));
            return _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}