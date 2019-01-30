using System;
using System.Threading.Tasks;

namespace AirView.Persistence.Core.Internal
{
    internal class DisposedUnitOfWorkParticipant :
        IUnitOfWorkParticipantState
    {
        private readonly string _participantName;

        public DisposedUnitOfWorkParticipant(string participantName) =>
            _participantName = participantName;

        public IUnitOfWorkParticipantState Commit(Action committer) =>
            throw new ObjectDisposedException(_participantName);

        public IUnitOfWorkParticipantState Dispose(Action disposer) => this;

        public IUnitOfWorkParticipantState Rollback(Action rollbacker) =>
            throw new ObjectDisposedException(_participantName);

        public Task<IUnitOfWorkParticipantState> SaveAsync(Func<Task> saver) =>
            throw new ObjectDisposedException(_participantName);
    }
}