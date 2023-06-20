using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Linq;
using TaskManager.DataAccess.Interfaces;

namespace TaskManager.DataAccess
{
    public class CosmosEnumerationBuilder : IEnumerationBuilder
    {
        public async Task<IEnumerable<TEntity>> EnumerateAsync<TEntity>(
            IQueryable<TEntity> query,
            CancellationToken cancellationToken)
        {
            var entities = new List<TEntity>();
            var feedIterator = query.ToFeedIterator();
            while (feedIterator.HasMoreResults)
            {
                entities.AddRange((await feedIterator.ReadNextAsync(cancellationToken)).Resource);
            }

            return entities;
        }
    }
}