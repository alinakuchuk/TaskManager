using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using TaskManager.DataAccess.Interfaces;
using TaskManager.DataAccess.Models;
using TaskManager.Services.Interfaces;
using TaskManager.Services.Models;

namespace TaskManager.Services.Services
{
    public sealed class QueryTaskService : IQueryTaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;

        public QueryTaskService(
            ITaskRepository taskRepository,
            IMapper mapper)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
        }
        
        public async Task<IEnumerable<DtoTask>> GetTasksAsync(
            DtoGetTasksQueryParameters queryParameters,
            CancellationToken cancellationToken)
        {
            var tasks = await _taskRepository.GetTasksAsync(
                _mapper.Map<DbGetTasksQueryParameters>(queryParameters),
                cancellationToken);
            
            return _mapper.Map<IEnumerable<DtoTask>>(tasks);
        }

        public async Task<DtoTask> GetTaskByIdAsync(
            Guid id,
            CancellationToken cancellationToken)
        {
            return _mapper.Map<DtoTask>(
                await _taskRepository.GetTaskByIdAsync(id, cancellationToken));
        }
    }
}