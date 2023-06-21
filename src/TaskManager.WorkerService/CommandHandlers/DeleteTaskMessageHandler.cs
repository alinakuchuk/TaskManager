using System;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TaskManager.Messaging;
using TaskManager.Messaging.Messages;
using TaskManager.Services.Interfaces;

namespace TaskManager.WorkerService.CommandHandlers
{
    public sealed class DeleteTaskMessageHandler : BackgroundService
    {
        private readonly ILogger<DeleteTaskMessageHandler> _logger;
        private readonly ServiceBusReceiver _serviceBusReceiver;
        private readonly IMessageSerialization<DeleteTaskMessage> _messageSerialization;
        private readonly IServiceProvider _serviceProvider;
        private readonly IMessageSender<DeleteTaskMessage> _messageSender;
        private readonly IMessageSender<DeleteTaskMessage> _errorSender;

        public DeleteTaskMessageHandler(
            ILogger<DeleteTaskMessageHandler> logger,
            ServiceBusReceiver serviceBusReceiver,
            IMessageSerialization<DeleteTaskMessage> messageSerialization,
            IServiceProvider serviceProvider,
            IMessageSender<DeleteTaskMessage> messageSender)
        {
            _logger = logger;
            _serviceBusReceiver = serviceBusReceiver;
            _messageSerialization = messageSerialization;
            _serviceProvider = serviceProvider;
            _messageSender = messageSender;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            ServiceBusReceivedMessage message = null;
            DeleteTaskMessage deleteTaskMessage = null;
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    message = await _serviceBusReceiver.ReceiveMessageAsync(cancellationToken: cancellationToken);
                    if (message != null)
                    {
                        deleteTaskMessage = _messageSerialization.Deserialize(message.Body.ToArray());
                        using (var scope = _serviceProvider.CreateScope())
                        {
                            var service = scope.ServiceProvider.GetRequiredService<ICommandTaskService>();
                            await service.DeleteTaskAsync(deleteTaskMessage.Id, cancellationToken);
                        }
                    
                        await _serviceBusReceiver.CompleteMessageAsync(message, cancellationToken);
                    }
                    
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }
                catch (Exception e)
                {
                    if (deleteTaskMessage != null)
                    {
                        await _messageSender.SendMessageAsync(deleteTaskMessage);
                    }
                    
                    _logger.LogError(e.Message);
                }
              
                await Task.Delay(500, cancellationToken);
            }
        }
    }
}