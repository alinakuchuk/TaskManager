using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TaskManager.Api.Query;
using TaskManager.Api.Queries;
using TaskManager.Services.Interfaces;
using TaskManager.Services.Models;

namespace TaskManager.Api.Handlers
{
    public sealed class GetTasksHandler : IRequestHandler<GetTasksQuery, IEnumerable<QueryTask>>
    {
        private readonly IQueryTaskService _queryTaskService;
        private readonly IMapper _mapper;

        public GetTasksHandler(
            IQueryTaskService queryTaskService,
            IMapper mapper)
        {
            _queryTaskService = queryTaskService;
            _mapper = mapper;
        }
            
        public async Task<IEnumerable<QueryTask>> Handle(
            GetTasksQuery query,
            CancellationToken cancellationToken)
        {
            var queryParameters = _mapper.Map<DtoGetTasksQueryParameters>(query);
            var tasks = await _queryTaskService.GetTasksAsync(
                queryParameters,
                cancellationToken);

            return _mapper.Map<IEnumerable<QueryTask>>(tasks);
        }
    }
}