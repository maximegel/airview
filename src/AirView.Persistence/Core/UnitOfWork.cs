using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AirView.Persistence.Core.Internal;
using AirView.Shared;

namespace AirView.Persistence.Core
{
    public sealed class UnitOfWork :
        IUnitOfWork,
        IUnitOfWorkContext
    {
        private readonly ICollection<IUnitOfWorkParticipant> _participants = new HashSet<IUnitOfWorkParticipant>();

        private readonly IDictionary<object, IUnitOfWorkParticipantState> _states =
            new Dictionary<object, IUnitOfWorkParticipantState>();

        private IUnitOfWorkState _state = new CommittableUnitOfWork();

        public void Dispose() =>
            _state = _state.Dispose(DisposeEach, RollbackEach);

        public async Task CommitAsync(CancellationToken cancellationToken) =>
            _state = await _state.CommitAsync(async () =>
            {
                try
                {
                    await SaveEachAsync(cancellationToken);
                    CommitEach();
                }
                catch (Exception e)
                {
                    RollbackEach();
                    throw new UnitOfWorkCommitException(
                        "Failed to commit data changes. Transactions have been rollbacked.", e);
                }
            });

        public void Enlist(IUnitOfWorkParticipant participant)
        {
            if (_participants.Contains(participant)) return;

            participant.Prepare();
            _participants.Add(participant);
            _states.Add(participant, new PreparedUnitOfWorkParticipant(participant.GetType().GetFriendlyName()));
        }

        private void CommitEach() =>
            ForEach((participant, state) => state.Commit(participant.Commit));

        private void DisposeEach() =>
            ForEach((participant, state) => state.Dispose(participant.Dispose));

        private void ForEach(
            Func<IUnitOfWorkParticipant, IUnitOfWorkParticipantState, IUnitOfWorkParticipantState> action)
        {
            foreach (var participant in _participants)
                _states[participant] = action(participant, _states[participant]);
        }

        private void RollbackEach() =>
            ForEach((participant, state) => state.Rollback(participant.Rollback));

        private Task SaveEachAsync(CancellationToken cancellationToken) =>
            Task.WhenAll(_participants.Select(async participant =>
                _states[participant] =
                    await _states[participant].SaveAsync(() => participant.SaveAsync(cancellationToken))));
    }

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

    internal class CommittedUnitOfWorkParticipant :
        IUnitOfWorkParticipantState
    {
        private readonly string _participantName;

        public CommittedUnitOfWorkParticipant(string participantName) =>
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

    internal interface IUnitOfWorkParticipantState
    {
        IUnitOfWorkParticipantState Commit(Action committer);

        IUnitOfWorkParticipantState Dispose(Action disposer);

        IUnitOfWorkParticipantState Rollback(Action rollbacker);

        Task<IUnitOfWorkParticipantState> SaveAsync(Func<Task> saver);
    }
}