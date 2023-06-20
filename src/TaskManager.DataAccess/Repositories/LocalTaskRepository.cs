using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskManager.DataAccess.Interfaces;
using TaskManager.DataAccess.Models;

namespace TaskManager.DataAccess.Repositories
{
    public sealed class LocalTaskRepository : ITaskRepository
    {
        private readonly ConcurrentDictionary<Guid, DbTask> _storage = new();

        public Task<IEnumerable<DbTask>> GetTasksAsync(
            DbGetTasksQueryParameters queryParameters,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(
                _storage.Values.AsEnumerable()
                    //IsDeleted
                    .Where(task => !task.IsDeleted)
                    //IsDone
                    .Where(task => !queryParameters.IsDone.HasValue ||
                                   task.IsDone == queryParameters.IsDone)
                    //DueData
                    .Where(task => !queryParameters.DueDateTime.HasValue ||
                                   task.DueDateTime == queryParameters.DueDateTime)
                    .Skip(queryParameters.Offset)
                    .Take(queryParameters.Limit));
        }

        public Task<DbTask> GetTaskByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return Task.FromResult(_storage.Values.FirstOrDefault(x => x.Id == id));
        }

        public Task CreateTaskAsync(DbTask task, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task UpdateTaskAsync(Guid id, DbTask task, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task DeleteTaskAsync(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}