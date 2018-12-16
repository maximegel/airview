using System.Threading;
using System.Threading.Tasks;

namespace AirView.Application.Core
{
    public delegate Task HandlerDelegate<in TSubject>(TSubject subject, CancellationToken cancellationToken);
}