namespace AirView.Domain.Core
{
    public interface IAggregateEvent<TAggregate, out TId> :
        IAggregateEvent
    {
    }
}