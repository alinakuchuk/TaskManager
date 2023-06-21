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
        private readonly IMessagingService<DeleteTaskMessage> _messagingService;

        public DeleteTaskHandler(IMessagingService<DeleteTaskMessage> messagingService)
        {
            _messagingService = messagingService;
        }

        public async Task Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            await _messagingService.SendMessageAsync(new DeleteTaskMessage
            {
                Id = request.Id
            });
        }
    }
}