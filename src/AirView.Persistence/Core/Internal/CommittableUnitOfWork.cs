using System;
using System.Threading.Tasks;

namespace AirView.Persistence.Core.Internal
{
    internal class CommittableUnitOfWork :
        IUnitOfWorkState
    {
        public async Task<IUnitOfWorkState> CommitAsync(Func<Task> committer)
        {
            await committer();
            return new CommittedUnitOfWork();
        }

        public IUnitOfWorkState Dispose(Action disposer, Action rollbacker)
        {
            rollbacker();
            disposer();
            return new DisposedUnitOfWork();
        }
    }
}