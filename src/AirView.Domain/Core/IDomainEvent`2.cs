namespace AirView.Domain.Core
{
    public interface IDomainEvent<TAggregate, out TAggregateId> :
        IDomainEvent
        where TAggregate : IAggregateRoot<TAggregate, TAggregateId>
    {
        new TAggregateId AggregateId { get; }

        new IAggregateEvent<TAggregate, TAggregateId> Data { get; }
    }
}