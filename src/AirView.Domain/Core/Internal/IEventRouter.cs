namespace AirView.Domain.Core.Internal
{
    internal interface IEventRouter<in TEvent>
    {
        void Dispatch(TEvent @event);
    }
}