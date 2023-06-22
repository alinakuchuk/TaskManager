using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TaskManager.Api.Commands;
using TaskManager.Messaging;
using TaskManager.Messaging.Messages;

namespace TaskManager.Api.Handlers
{
    public sealed class CreateTaskHandler: IRequestHandler<CreateTaskCommand>
    {
        private readonly IMessageSender<CreateTaskMessage> _messageSender;
        private readonly IMapper _mapper;

        public CreateTaskHandler(IMessageSender<CreateTaskMessage> messageSender, IMapper mapper)
        {
            _messageSender = messageSender;
            _mapper = mapper;
        }

        public async Task Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            await _messageSender.SendMessageAsync(_mapper.Map<CreateTaskMessage>(request));
        }
    }
}