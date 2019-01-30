using System;
using System.Threading.Tasks;

namespace AirView.Persistence.Core.Internal
{
    internal class CommittedUnitOfWork :
        IUnitOfWorkState
    {
        public Task<IUnitOfWorkState> CommitAsync(Func<Task> committer) =>
            throw new UnitOfWorkCommittedTwiceException(
                "You can't call CommitAsync() more than once on a unit of work. " +
                "All the data changes managed by this unit of work have already been saved and all transactions have been completed and closed. " +
                "If you wish to make more data changes, create a new unit of work and make your changes there.");

        public IUnitOfWorkState Dispose(Action disposer, Action rollbacker)
        {
            disposer();
            return new DisposedUnitOfWork();
        }
    }
}