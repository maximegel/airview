namespace AirView.Persistence.Core.EventSourcing
{
    public interface IEventLog<TEvent> :
        IUnitOfWorkParticipant
    {
        IEventStream<TEvent> GetStream(object id);
    }
}