using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TaskManager.Api.Commands;
using TaskManager.Messaging;
using TaskManager.Messaging.Messages;

namespace TaskManager.Api.Handlers
{
    public sealed class CreateTaskHandler: IRequestHandler<CreateTaskCommand>
    {
        private readonly IMessagingService<CreateTaskMessage> _messagingService;

        public CreateTaskHandler(IMessagingService<CreateTaskMessage> messagingService)
        {
            _messagingService = messagingService;
        }

        public async Task Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            await _messagingService.SendMessageAsync(new CreateTaskMessage
            {
                Name = request.Task.Name,
                Description = request.Task.Description,
                DueDateTime = DateTime.Parse(request.Task.DueDateTime),
                IsDone = request.Task.IsDone
            });
        }
    }
}