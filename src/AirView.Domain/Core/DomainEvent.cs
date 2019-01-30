using System;

namespace AirView.Domain.Core
{
    public static class DomainEvent
    {
        public static IDomainEvent Of<TAggregate>(
            object aggregateId, long aggregateVersion, Type dataType, object data)
            where TAggregate : IAggregateRoot =>
            (IDomainEvent<TAggregate>) Activator.CreateInstance(
                typeof(DomainEvent<,>).MakeGenericType(typeof(TAggregate), dataType),
                aggregateId, aggregateVersion, data);

        public static IDomainEvent<TAggregate> Of<TAggregate>(
            object aggregateId, long aggregateVersion, IAggregateEvent data)
            where TAggregate : IAggregateRoot =>
            new DomainEvent<TAggregate, IAggregateEvent>(aggregateId, aggregateVersion, data);

        public static IDomainEvent<TAggregate, TAggregateEvent> Of<TAggregate, TAggregateEvent>(
            object aggregateId, long aggregateVersion, TAggregateEvent data)
            where TAggregate : IAggregateRoot
            where TAggregateEvent : IAggregateEvent =>
            new DomainEvent<TAggregate, TAggregateEvent>(aggregateId, aggregateVersion, data);

        public static IDomainEvent Of<TAggregateEvent>(
            Type aggregateType, object aggregateId, long aggregateVersion, TAggregateEvent data)
            where TAggregateEvent : IAggregateEvent =>
            (IDomainEvent) Activator.CreateInstance(
                typeof(DomainEvent<,>).MakeGenericType(aggregateType, typeof(TAggregateEvent)),
                aggregateId, aggregateVersion, data);
    }
}