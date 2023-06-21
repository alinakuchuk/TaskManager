using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TaskManager.Api.Commands;
using TaskManager.Messaging;
using TaskManager.Messaging.Messages;

namespace TaskManager.Api.Handlers
{
    public sealed class UpdateTaskHandler : IRequestHandler<UpdateTaskCommand>
    {
        private readonly IMessagingService<UpdateTaskMessage> _messagingService;

        public UpdateTaskHandler(IMessagingService<UpdateTaskMessage> messagingService)
        {
            _messagingService = messagingService;
        }
        
        public async Task Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            await _messagingService.SendMessageAsync(new UpdateTaskMessage
            {
                Id = request.Id,
                Name = request.Task.Name,
                Description = request.Task.Description,
                DueDateTime = DateTime.Parse(request.Task.DueDateTime),
                IsDone = request.Task.IsDone
            });
        }
    }
}