using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using TaskManager.DataAccess.Interfaces;
using TaskManager.DataAccess.Models;

namespace TaskManager.DataAccess.Repositories
{
    public sealed class TaskRepository : ITaskRepository
    {
        private readonly Container _container;
        private readonly IEnumerationBuilder _enumerationBuilder;
        
        public TaskRepository(Container container, IEnumerationBuilder enumerationBuilder)
        {
            _container = container;
            _enumerationBuilder = enumerationBuilder;
        }

        public async Task<IEnumerable<DbTask>> GetTasksAsync(
            DbGetTasksQueryParameters queryParameters,
            CancellationToken cancellationToken)
        {
            var queryable = _container.GetItemLinqQueryable<DbTask>()
                //IsDeleted
                .Where(task => !task.IsDeleted)
                //IsDone
                .Where(task => !queryParameters.IsDone.HasValue ||
                               task.IsDone == queryParameters.IsDone)
                //DueData
                .Where(task => !queryParameters.DueDateTime.HasValue ||
                               task.DueDateTime == queryParameters.DueDateTime)
                .Skip(queryParameters.Offset)
                .Take(queryParameters.Limit);

            return await _enumerationBuilder.EnumerateAsync(
                queryable, 
                cancellationToken);
        }

        public async Task<DbTask> GetTaskByIdAsync(
            Guid id,
            CancellationToken cancellationToken)
        {
            var stringId = id.ToString();
            
            return (await _container.ReadItemAsync<DbTask>(
                stringId,
                new PartitionKey(stringId),
                cancellationToken: cancellationToken)).Resource;
        }

        public async Task CreateTaskAsync(
            DbTask task,
            CancellationToken cancellationToken)
        {
            task.Id = Guid.NewGuid();
            task.CreatedDateTime = DateTime.UtcNow;

            await _container.CreateItemAsync(
                task,
                cancellationToken: cancellationToken);
        }

        public async Task UpdateTaskAsync(
            Guid id,
            DbTask task,
            CancellationToken cancellationToken)
        {
            await _container.ReplaceItemAsync(
                task,
                id.ToString(),
                cancellationToken: cancellationToken);
        }

        public async Task DeleteTaskAsync(
            Guid id,
            CancellationToken cancellationToken)
        {
            var stringId = id.ToString();
            var targetStory = (await _container.ReadItemAsync<DbTask>(
                stringId,
                new PartitionKey(stringId),
                cancellationToken: cancellationToken)).Resource;
            
            targetStory.DeletedDateTime = DateTime.UtcNow;
            targetStory.IsDeleted = true;
            
            await _container.ReplaceItemAsync(
                targetStory, 
                stringId, 
                cancellationToken: cancellationToken);
        }
    }
}