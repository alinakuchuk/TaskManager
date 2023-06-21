using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TaskManager.Api.Commands;
using TaskManager.Messaging;
using TaskManager.Messaging.Messages;

namespace TaskManager.Api.Handlers
{
    public sealed class DeleteTaskHandler : IRequestHandler<DeleteTaskCommand>
    {
        private readonly IMessageSender<DeleteTaskMessage> _messageSender;

        public DeleteTaskHandler(IMessageSender<DeleteTaskMessage> messageSender)
        {
            _messageSender = messageSender;
        }

        public async Task Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            await _messageSender.SendMessageAsync(new DeleteTaskMessage
            {
                Id = request.Id
            });
        }
    }
}