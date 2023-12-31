using System;
using System.Threading.Tasks;
using Grpc.Core;
using MediatR;
using TaskManager.Api.Queries;
using TaskManager.Api.Query;

namespace TaskManager.Api.Services
{
    public sealed class QueryManagedTaskService : QueryTaskService.QueryTaskServiceBase
    {
        private readonly ISender _sender;

        public QueryManagedTaskService(ISender sender)
        {
            _sender = sender;
        }
        
        public override async Task<GetTasksResponse> QueryTasks(
            GetTasksRequest request,
            ServerCallContext context)
        {
            var query = new GetTasksQuery(
                DateTime.TryParse(request.DueDateTime, out var value) ? value : null,
                request.IsDone,
                request.Limit,
                request.Offset);

            var tasks = await _sender.Send(query);
            var response = new GetTasksResponse();
            response.Tasks.AddRange(tasks);
            
            return response;
        }
        
        public override async Task<GetTaskResponse> QueryById(
            GetTaskRequest request,
            ServerCallContext context)
        {
            var task = await _sender.Send(new GetTaskByIdQuery(Guid.Parse(request.Id)));
            
            return new GetTaskResponse { Task = task };
        }
    }
}