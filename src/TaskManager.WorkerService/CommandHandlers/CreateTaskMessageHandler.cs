using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TaskManager.Messaging;
using TaskManager.Messaging.Messages;
using TaskManager.Services.Interfaces;
using TaskManager.Services.Models;

namespace TaskManager.WorkerService.CommandHandlers
{
    public sealed class CreateTaskMessageHandler : BackgroundService
    {
        private readonly ILogger<CreateTaskMessageHandler> _logger;
        private readonly ServiceBusReceiver _serviceBusReceiver;
        private readonly IMessageSerialization<CreateTaskMessage> _messageSerialization;
        private readonly ICommandTaskService _commandTaskService;
        private readonly IMapper _mapper;

        public CreateTaskMessageHandler(
            ILogger<CreateTaskMessageHandler> logger,
            ServiceBusReceiver serviceBusReceiver,
            IMessageSerialization<CreateTaskMessage> messageSerialization,
            ICommandTaskService commandTaskService,
            IMapper mapper)
        {
            _logger = logger;
            _serviceBusReceiver = serviceBusReceiver;
            _messageSerialization = messageSerialization;
            _commandTaskService = commandTaskService;
            _mapper = mapper;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var message = await _serviceBusReceiver.ReceiveMessageAsync(cancellationToken: cancellationToken);
                    var createTaskMessage = _messageSerialization.Deserialize(message.Body.ToArray());
                    var dtoTask = _mapper.Map<DtoTask>(createTaskMessage);

                    await _commandTaskService.CreateTaskAsync(dtoTask, cancellationToken);

                    await _serviceBusReceiver.CompleteMessageAsync(message, cancellationToken);
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                }
              
                await Task.Delay(500, cancellationToken);
            }
        }
    }
}