using System;
using System.Threading;
using System.Threading.Tasks;
using TaskManager.Services.Models;

namespace TaskManager.Services.Interfaces
{
    public interface ICommandTaskService
    {
        Task CreateTaskAsync(DtoTask task, CancellationToken cancellationToken);
        
        Task UpdateTaskAsync(Guid id, DtoTask task, CancellationToken cancellationToken);
        
        Task DeleteTaskAsync(Guid id, CancellationToken cancellationToken);
    }
}