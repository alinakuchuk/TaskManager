using System;
using System.Threading.Tasks;
using AutoMapper;
using Grpc.Core;
using MediatR;
using TaskManager.Api.Queries;
using TaskManager.Api.Query;

namespace TaskManager.Api.Services
{
    public sealed class QueryManagedTaskService : QueryTaskService.QueryTaskServiceBase
    {
        private readonly ISender _sender;
        private readonly IMapper _mapper;

        public QueryManagedTaskService(ISender sender, IMapper mapper)
        {
            _sender = sender;
            _mapper = mapper;
        }
        
        public override async Task<GetTasksResponse> QueryTasks(
            GetTasksRequest request,
            ServerCallContext context)
        {
            var query = new GetTasksQuery
            {
                DueDateTime = DateTime.TryParse(request.DueDateTime, out var value)
                    ? value
                    : null,
                IsDone = request.IsDone,
                Limit = request.Limit,
                Offset = request.Offset
            };

            var tasks = await _sender.Send(query);
            var response = new GetTasksResponse();
            response.Tasks.AddRange(tasks);
            
            return response;
        }
        
        public override async Task<GetTaskResponse> QueryById(
            GetTaskRequest request,
            ServerCallContext context)
        {
            var query = _mapper.Map<GetTaskByIdQuery>(request);
            var task = await _sender.Send(query);
            
            return new GetTaskResponse
            {
                Task = task
            };
        }
    }
}