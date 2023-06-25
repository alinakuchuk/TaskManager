using System;
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
            await _sender.Send(new CreateTaskCommand(request.Task));
            
            return new CreateTaskResponse();
        }
        
        public override async Task<UpdateTaskResponse> UpdateTask(
            UpdateTaskRequest request,
            ServerCallContext context)
        {
            await _sender.Send(new UpdateTaskCommand(Guid.Parse(request.Id), request.Task));
            
            return new UpdateTaskResponse();
        }

        public override async Task<DeleteTaskResponse> DeleteTask(
            DeleteTaskRequest request,
            ServerCallContext context)
        {
            await _sender.Send(new DeleteTaskCommand(Guid.Parse(request.Id)));
            
            return new DeleteTaskResponse();
        }
    }
}