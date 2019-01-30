using System;
using System.Threading.Tasks;

namespace AirView.Persistence.Core.Internal
{
    internal interface IUnitOfWorkState
    {
        Task<IUnitOfWorkState> CommitAsync(Func<Task> committer);

        IUnitOfWorkState Dispose(Action disposer, Action rollbacker);
    }
}