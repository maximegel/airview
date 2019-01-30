using System;
using System.Threading.Tasks;

namespace AirView.Persistence.Core.Internal
{
    internal class DisposedUnitOfWork :
        IUnitOfWorkState
    {
        public Task<IUnitOfWorkState> CommitAsync(Func<Task> committer) =>
            throw new ObjectDisposedException(nameof(UnitOfWork));

        public IUnitOfWorkState Dispose(Action disposer, Action rollbacker) => this;
    }
}