using System;

namespace AirView.Domain.Core
{
    public class DomainEvent<TAggregate, TAggregateId, TAggregateEvent> :
        IDomainEvent<TAggregate, TAggregateId, TAggregateEvent>
        where TAggregate : IAggregateRoot<TAggregate, TAggregateId>
        where TAggregateEvent : IAggregateEvent<TAggregate, TAggregateId>
    {
        public DomainEvent(TAggregateId aggregateId, long aggregateVersion, TAggregateEvent data)
        {
            AggregateId = aggregateId;
            AggregateType = typeof(TAggregate);
            AggregateVersion = aggregateVersion;
            Data = data;
        }

        object IDomainEvent.AggregateId => AggregateId;

        public Type AggregateType { get; }

        public long AggregateVersion { get; }

        IAggregateEvent IDomainEvent.Data => Data;

        public TAggregateEvent Data { get; }

        public TAggregateId AggregateId { get; }

        IAggregateEvent<TAggregate, TAggregateId> IDomainEvent<TAggregate, TAggregateId>.Data => Data;
    }
}