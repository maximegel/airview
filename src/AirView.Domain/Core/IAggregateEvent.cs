namespace AirView.Domain.Core
{
    public interface IAggregateEvent
    {
        string Name { get; }
    }
}