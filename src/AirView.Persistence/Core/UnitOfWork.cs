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
}