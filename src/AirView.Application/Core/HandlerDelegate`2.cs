using System.Threading;
using System.Threading.Tasks;

namespace AirView.Application.Core
{
    public delegate Task<TResult> HandlerDelegate<in TSubject, TResult>(
        TSubject subject, CancellationToken cancellationToken);
}