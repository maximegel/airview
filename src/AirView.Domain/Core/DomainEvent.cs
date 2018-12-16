namespace AirView.Domain.Core
{
    public static class DomainEvent
    {
        public static IDomainEvent<TAggregate, TAggregateId> Of<
            TAggregate, TAggregateId>(
            TAggregate aggregate, IAggregateEvent<TAggregate, TAggregateId> data)
            where TAggregate : IAggregateRoot<TAggregate, TAggregateId> =>
            Of<TAggregate, TAggregateId, IAggregateEvent<TAggregate, TAggregateId>>(aggregate, data);

        public static IDomainEvent<TAggregate, TAggregateId> Of<
            TAggregate, TAggregateId>(
            TAggregateId aggregateId, long aggregateVersion, IAggregateEvent<TAggregate, TAggregateId> data)
            where TAggregate : IAggregateRoot<TAggregate, TAggregateId> =>
            Of<TAggregate, TAggregateId, IAggregateEvent<TAggregate, TAggregateId>>(
                aggregateId, aggregateVersion, data);

        public static IDomainEvent<TAggregate, TAggregateId, TAggregateEvent> Of<
            TAggregate, TAggregateId, TAggregateEvent>(
            TAggregate aggregate, TAggregateEvent data)
            where TAggregate : IAggregateRoot<TAggregate, TAggregateId>
            where TAggregateEvent : IAggregateEvent<TAggregate, TAggregateId> =>
            Of<TAggregate, TAggregateId, TAggregateEvent>(aggregate.Id, aggregate.Version, data);

        public static IDomainEvent<TAggregate, TAggregateId, TAggregateEvent> Of<
            TAggregate, TAggregateId, TAggregateEvent>(
            TAggregateId aggregateId, long aggregateVersion, TAggregateEvent data)
            where TAggregate : IAggregateRoot<TAggregate, TAggregateId>
            where TAggregateEvent : IAggregateEvent<TAggregate, TAggregateId> =>
            new DomainEvent<TAggregate, TAggregateId, TAggregateEvent>(aggregateId, aggregateVersion, data);
    }
}