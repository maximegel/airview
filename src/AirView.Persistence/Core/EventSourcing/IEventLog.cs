namespace AirView.Persistence.Core.EventSourcing
{
    public interface IEventLog<TEvent> :
        IUnitOfWorkParticipant
    {
        IEventStream<TEvent> Stream(object id);
    }
}