using System;

namespace AirView.Domain.Core
{
    public class DomainEvent<TAggregate, TAggregateEvent> :
        IDomainEvent<TAggregate, TAggregateEvent>
        where TAggregate : IAggregateRoot
        where TAggregateEvent : IAggregateEvent
    {
        public DomainEvent(object aggregateId, long aggregateVersion, TAggregateEvent data)
        {
            AggregateId = aggregateId;
            AggregateType = typeof(TAggregate);
            AggregateVersion = aggregateVersion;
            Data = data;
        }

        public object AggregateId { get; }

        public Type AggregateType { get; }

        public long AggregateVersion { get; }

        IAggregateEvent IDomainEvent.Data => Data;

        public TAggregateEvent Data { get; }
    }
}