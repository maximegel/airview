using System.Threading;
using System.Threading.Tasks;

namespace AirView.Application.Core
{
    public interface ICommandSender
    {
        Task<TResult> SendAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default);
    }
}