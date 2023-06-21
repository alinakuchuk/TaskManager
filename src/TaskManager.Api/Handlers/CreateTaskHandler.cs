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
        private readonly IMessageSender<CreateTaskMessage> _messageSender;

        public CreateTaskHandler(IMessageSender<CreateTaskMessage> messageSender)
        {
            _messageSender = messageSender;
        }

        public async Task Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            await _messageSender.SendMessageAsync(new CreateTaskMessage
            {
                Name = request.Task.Name,
                Description = request.Task.Description,
                DueDateTime = DateTime.Parse(request.Task.DueDateTime),
                IsDone = request.Task.IsDone
            });
        }
    }
}