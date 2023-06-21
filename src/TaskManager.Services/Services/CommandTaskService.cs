using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using TaskManager.DataAccess.Interfaces;
using TaskManager.DataAccess.Models;
using TaskManager.Services.Interfaces;
using TaskManager.Services.Models;

namespace TaskManager.Services.Services
{
    public sealed class CommandTaskService : ICommandTaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;

        public CommandTaskService(ITaskRepository taskRepository, IMapper mapper)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
        }

        public async Task CreateTaskAsync(DtoTask task, CancellationToken cancellationToken)
        {
            var dbTask = _mapper.Map<DbTask>(task);
            await _taskRepository.CreateTaskAsync(dbTask, cancellationToken);
        }

        public async Task UpdateTaskAsync(Guid id, DtoTask task, CancellationToken cancellationToken)
        {
            var dbTask = _mapper.Map<DbTask>(task);
            await _taskRepository.UpdateTaskAsync(id, dbTask, cancellationToken);
        }

        public async Task DeleteTaskAsync(Guid id, CancellationToken cancellationToken)
        {
            await _taskRepository.DeleteTaskAsync(id, cancellationToken);
        }
    }
}