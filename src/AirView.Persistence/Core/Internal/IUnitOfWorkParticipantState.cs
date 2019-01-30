using System;
using System.Threading.Tasks;

namespace AirView.Persistence.Core.Internal
{
    internal interface IUnitOfWorkParticipantState
    {
        IUnitOfWorkParticipantState Commit(Action committer);

        IUnitOfWorkParticipantState Dispose(Action disposer);

        IUnitOfWorkParticipantState Rollback(Action rollbacker);

        Task<IUnitOfWorkParticipantState> SaveAsync(Func<Task> saver);
    }
}