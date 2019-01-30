using System;
using System.Threading.Tasks;
using AirView.Persistence.Core.Internal;

namespace AirView.Persistence.Core.Internal
{
    internal class PreparedUnitOfWorkParticipant :
        IUnitOfWorkParticipantState
    {
        private readonly string _participantName;

        public PreparedUnitOfWorkParticipant(string participantName) =>
            _participantName = participantName;

        public IUnitOfWorkParticipantState Commit(Action committer) => this;

        public IUnitOfWorkParticipantState Dispose(Action disposer)
        {
            disposer();
            return new DisposedUnitOfWorkParticipant(_participantName);
        }

        public IUnitOfWorkParticipantState Rollback(Action rollbacker) => this;

        public async Task<IUnitOfWorkParticipantState> SaveAsync(Func<Task> saver)
        {
            await saver();
            return new SavedUnitOfWorkParticipant(_participantName);
        }
    }
}