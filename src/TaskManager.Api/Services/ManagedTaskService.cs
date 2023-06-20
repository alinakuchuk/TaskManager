using System;
using System.Threading.Tasks;
using Grpc.Core;
using MediatR;
using TaskManager.Api.Queries;
using TaskManager.Api.Query;

namespace TaskManager.Api.Services
{
    public sealed class ManagedTaskService : QueryTaskService.QueryTaskServiceBase
    {
        private readonly ISender _sender;

        public ManagedTaskService(ISender sender)
        {
            _sender = sender;
        }
        
        public override async Task<TasksResponse> QueryTasks(
            TasksRequest request,
            ServerCallContext context)
        {
            var query = new GetTasksQuery
            {
                DueData = DateTime.TryParse(request.DueDateTime, out var value)
                    ? value
                    : null,
                IsDone = request.IsDone,
                Limit = request.Limit,
                Offset = request.Offset
            };

            var tasks = await _sender.Send(query);
            var response = new TasksResponse();
            response.Tasks.AddRange(tasks);
            
            return response;
        }
        
        public override async Task<TaskResponse> QueryById(
            TaskRequest request,
            ServerCallContext context)
        {
            var query = new GetTaskByIdQuery
            {
                Id = Guid.Parse(request.Id)
            };

            var task = await _sender.Send(query);
            
            return new TaskResponse
            {
                Task = task
            };
        }
    }
}