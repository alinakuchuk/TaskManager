using System.Threading.Tasks;
using Grpc.Core;
using MediatR;
using TaskManager.Api.Query;

namespace TaskManager.Api.Services
{
    public sealed class ManagedTaskService : QueryTaskService.QueryTaskServiceBase
    {
        private readonly IMediator _mediator;
        
        public override Task<TasksResponse> QueryTasks(TasksRequest request, ServerCallContext context)
        {
            return base.QueryTasks(request, context);
        }
        
        public override Task<TaskResponse> QueryById(TaskRequest request, ServerCallContext context)
        {
            var taskResponse = new TaskResponse();
            return Task.FromResult(taskResponse);
        }
    }
}