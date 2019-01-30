using System;
using System.Threading.Tasks;

namespace AirView.Persistence.Core.Internal
{
    internal class SavedUnitOfWorkParticipant :
        IUnitOfWorkParticipantState
    {
        private readonly string _participantName;

        public SavedUnitOfWorkParticipant(string participantName) =>
            _participantName = participantName;

        public IUnitOfWorkParticipantState Commit(Action committer)
        {
            committer();
            return new CommittedUnitOfWorkParticipant(_participantName);
        }

        public IUnitOfWorkParticipantState Dispose(Action disposer) => this;

        public IUnitOfWorkParticipantState Rollback(Action rollbacker)
        {
            rollbacker();
            return new RollbackedUnitOfWorkParticipant(_participantName);
        }

        public async Task<IUnitOfWorkParticipantState> SaveAsync(Func<Task> saver)
        {
            await saver();
            return this;
        }
    }
}