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
    public sealed class DeleteTaskMessageHandler : BackgroundService
    {
        private readonly ILogger<DeleteTaskMessageHandler> _logger;
        private readonly ServiceBusReceiver _serviceBusReceiver;
        private readonly IMessageSerialization<DeleteTaskMessage> _messageSerialization;
        private readonly ICommandTaskService _commandTaskService;

        public DeleteTaskMessageHandler(
            ILogger<DeleteTaskMessageHandler> logger,
            ServiceBusReceiver serviceBusReceiver,
            IMessageSerialization<DeleteTaskMessage> messageSerialization,
            ICommandTaskService commandTaskService)
        {
            _logger = logger;
            _serviceBusReceiver = serviceBusReceiver;
            _messageSerialization = messageSerialization;
            _commandTaskService = commandTaskService;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var message = await _serviceBusReceiver.ReceiveMessageAsync(cancellationToken: cancellationToken);
                    var deleteTaskMessage = _messageSerialization.Deserialize(message.Body.ToArray());

                    await _commandTaskService.DeleteTaskAsync(deleteTaskMessage.Id, cancellationToken);

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