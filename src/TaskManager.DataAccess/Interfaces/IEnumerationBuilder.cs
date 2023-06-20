using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TaskManager.DataAccess.Interfaces
{
    public interface IEnumerationBuilder
    {
        Task<IEnumerable<TEntity>> EnumerateAsync<TEntity>(
            IQueryable<TEntity> query,
            CancellationToken cancellationToken);
    }
}