using System.Threading.Tasks;
using AutoMapper;
using Grpc.Core;
using MediatR;
using TaskManager.Api.Command;
using TaskManager.Api.Commands;

namespace TaskManager.Api.Services
{
    public sealed class CommandManagedTaskService : CommandTaskService.CommandTaskServiceBase
    {
        private readonly ISender _sender;
        private readonly IMapper _mapper;

        public CommandManagedTaskService(ISender sender, IMapper mapper)
        {
            _sender = sender;
            _mapper = mapper;
        }
        
        public override async Task<CreateTaskResponse> CreateTask(
            CreateTaskRequest request,
            ServerCallContext context)
        {
            var command = _mapper.Map<CreateTaskCommand>(request);
            await _sender.Send(command);
            
            return new CreateTaskResponse();
        }
        
        public override async Task<UpdateTaskResponse> UpdateTask(
            UpdateTaskRequest request,
            ServerCallContext context)
        {
            var command = _mapper.Map<UpdateTaskCommand>(request);
            await _sender.Send(command);
            
            return new UpdateTaskResponse();
        }

        public override async Task<DeleteTaskResponse> DeleteTask(
            DeleteTaskRequest request,
            ServerCallContext context)
        {
            var command = _mapper.Map<DeleteTaskCommand>(request);
            await _sender.Send(command);
            
            return new DeleteTaskResponse();
        }
    }
}