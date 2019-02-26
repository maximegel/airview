using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AirView.Persistence.Core.EventSourcing
{
    public interface IEventReader<TEvent>
    {
        Task<IEnumerable<TEvent>> ReadAsync(
            object streamId, long startIndex, int limit, CancellationToken cancellationToken = default);
    }
}