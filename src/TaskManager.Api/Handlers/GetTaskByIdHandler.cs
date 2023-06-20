using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TaskManager.Api.Queries;
using TaskManager.Api.Query;
using TaskManager.Services.Interfaces;

namespace TaskManager.Api.Handlers
{
    public sealed class GetTaskByIdHandler : IRequestHandler<GetTaskByIdQuery, ManagedTask>
    {
        private readonly IQueryTaskService _queryTaskService;
        private readonly IMapper _mapper;

        public GetTaskByIdHandler(
            IQueryTaskService queryTaskService,
            IMapper mapper)
        {
            _queryTaskService = queryTaskService;
            _mapper = mapper;
        }
            
        public async Task<ManagedTask> Handle(
            GetTaskByIdQuery query,
            CancellationToken cancellationToken)
        {
            var task = await _queryTaskService.GetTaskByIdAsync(
                query.Id,
                cancellationToken);

            return _mapper.Map<ManagedTask>(task);
        }
    }
}
