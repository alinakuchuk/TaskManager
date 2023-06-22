using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TaskManager.Api.Commands;
using TaskManager.Messaging;
using TaskManager.Messaging.Messages;

namespace TaskManager.Api.Handlers
{
    public sealed class UpdateTaskHandler : IRequestHandler<UpdateTaskCommand>
    {
        private readonly IMessageSender<UpdateTaskMessage> _messageSender;
        private readonly IMapper _mapper;

        public UpdateTaskHandler(IMessageSender<UpdateTaskMessage> messageSender, IMapper mapper)
        {
            _messageSender = messageSender;
            _mapper = mapper;
        }
        
        public async Task Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            await _messageSender.SendMessageAsync(_mapper.Map<UpdateTaskMessage>(request));
        }
    }
}