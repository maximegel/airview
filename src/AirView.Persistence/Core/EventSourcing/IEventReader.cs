using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AirView.Persistence.Core.EventSourcing
{
    public interface IEventReader<TEvent>
    {
        Task<IEnumerable<TEvent>> ReadAsync(
            object streamId, int limit, long offset = 0, CancellationToken cancellationToken = default);
    }
}