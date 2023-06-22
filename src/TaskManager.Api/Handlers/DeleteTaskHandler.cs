using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TaskManager.Api.Commands;
using TaskManager.Messaging;
using TaskManager.Messaging.Messages;

namespace TaskManager.Api.Handlers
{
    public sealed class DeleteTaskHandler : IRequestHandler<DeleteTaskCommand>
    {
        private readonly IMessageSender<DeleteTaskMessage> _messageSender;
        private readonly IMapper _mapper;
        
        public DeleteTaskHandler(IMessageSender<DeleteTaskMessage> messageSender, IMapper mapper)
        {
            _messageSender = messageSender;
            _mapper = mapper;
        }

        public async Task Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            await _messageSender.SendMessageAsync(_mapper.Map<DeleteTaskMessage>(request));
        }
    }
}