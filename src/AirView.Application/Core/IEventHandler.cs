using System.Threading;
using System.Threading.Tasks;
using AirView.Domain.Core;

namespace AirView.Application.Core
{
    public interface IEventHandler<in TEvent>
        where TEvent : IDomainEvent
    {
        Task HandleAsync(TEvent @event, CancellationToken cancellationToken = default);
    }
}