using System;
using System.Threading;
using System.Threading.Tasks;

namespace AirView.Persistence.Core
{
    public interface IUnitOfWork :
        IDisposable
    {
        Task CommitAsync(CancellationToken cancellationToken = default);
    }
}