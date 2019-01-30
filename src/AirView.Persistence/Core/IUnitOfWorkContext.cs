namespace AirView.Persistence.Core
{
    public interface IUnitOfWorkContext
    {
        void Enlist(IUnitOfWorkParticipant participant);
    }
}