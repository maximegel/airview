using System;
using System.Threading.Tasks;

namespace AirView.Persistence.Core.Internal
{
    internal class RollbackedUnitOfWorkParticipant :
        IUnitOfWorkParticipantState
    {
        private readonly string _participantName;

        public RollbackedUnitOfWorkParticipant(string participantName) =>
            _participantName = participantName;

        public IUnitOfWorkParticipantState Commit(Action committer) => this;

        public IUnitOfWorkParticipantState Dispose(Action disposer)
        {
            disposer();
            return new DisposedUnitOfWorkParticipant(_participantName);
        }

        public IUnitOfWorkParticipantState Rollback(Action rollbacker) => this;

        public Task<IUnitOfWorkParticipantState> SaveAsync(Func<Task> saver) => 
            Task.FromResult(this as IUnitOfWorkParticipantState);
    }
}