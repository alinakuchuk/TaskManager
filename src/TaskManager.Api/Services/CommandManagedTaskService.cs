using System.Threading.Tasks;
using Grpc.Core;
using MediatR;
using TaskManager.Api.Command;
using TaskManager.Api.Commands;

namespace TaskManager.Api.Services
{
    public sealed class CommandManagedTaskService : CommandTaskService.CommandTaskServiceBase
    {
        private readonly ISender _sender;

        public CommandManagedTaskService(ISender sender)
        {
            _sender = sender;
        }
        
        public override async Task<CreateTaskResponse> CreateTask(
            CreateTaskRequest request,
            ServerCallContext context)
        {
            var command = new CreateTaskCommand
            {
                Task = request.Task
            };

            await _sender.Send(command);
            
            return new CreateTaskResponse();
        }
    }
}