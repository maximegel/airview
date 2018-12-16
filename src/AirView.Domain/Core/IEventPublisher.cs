using System.Threading;
using System.Threading.Tasks;

namespace AirView.Domain.Core
{
    public interface IEventPublisher
    {
        Task PublishAsync(IDomainEvent @event, CancellationToken cancellationToken = default);
    }
}