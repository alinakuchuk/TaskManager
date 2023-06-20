using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TaskManager.Services.Models;

namespace TaskManager.Services.Interfaces
{
    public interface IQueryTaskService
    {
        Task<IEnumerable<DtoTask>> GetTasksAsync(
            DtoGetTasksQueryParameters queryParameters,
            CancellationToken cancellationToken);

        Task<DtoTask> GetTaskByIdAsync(Guid id, CancellationToken cancellationToken); 
    }
}