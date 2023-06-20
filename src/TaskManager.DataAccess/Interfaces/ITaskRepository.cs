using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TaskManager.DataAccess.Models;

namespace TaskManager.DataAccess.Interfaces
{
    public interface ITaskRepository
    {
        Task<IEnumerable<DbTask>> GetTasksAsync(
            DbGetTasksQueryParameters queryParameters,
            CancellationToken cancellationToken);

        Task<DbTask> GetTaskByIdAsync(Guid id, CancellationToken cancellationToken);

        Task CreateTaskAsync(DbTask task, CancellationToken cancellationToken);
        
        Task UpdateTaskAsync(Guid id, DbTask task, CancellationToken cancellationToken);
        
        Task DeleteTaskAsync(Guid id, CancellationToken cancellationToken);
    }
}