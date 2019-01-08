using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AirView.Api.Core
{
    public interface IPagedQueryable<out T> :
        IQueryable<T>
    {
        int Limit { get; }

        int Offset { get; }

        Task<int> TotalCountAsync(CancellationToken cancellationToken = default);
    }
}