using System;
using System.Threading;
using System.Threading.Tasks;

namespace AirView.Persistence.Core
{
    public interface IUnitOfWorkParticipant :
        IDisposable
    {
        void Commit();

        void Prepare();

        void Rollback();

        Task SaveAsync(CancellationToken cancellationToken = default);
    }
}