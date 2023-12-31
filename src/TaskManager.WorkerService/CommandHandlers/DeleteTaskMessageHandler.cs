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

        public DeleteTaskMessageHandler(
            ILogger<DeleteTaskMessageHandler> logger,
            ServiceBusReceiver serviceBusReceiver,
            IMessageSerialization<DeleteTaskMessage> messageSerialization,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceBusReceiver = serviceBusReceiver;
            _messageSerialization = messageSerialization;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                DeleteTaskMessage deleteTaskMessage = null;
                try
                {
                    var message = await _serviceBusReceiver.ReceiveMessageAsync(cancellationToken: cancellationToken);
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
                    
                    _logger.LogInformation("Worker running at: {time}", DateTime.UtcNow);
                }
                catch (Exception e)
                {
                    if (deleteTaskMessage != null)
                    {
                        using var scope = _serviceProvider.CreateScope();
                        var sender = scope.ServiceProvider.GetRequiredService<IMessageSender<DeleteTaskMessage>>();
                        await sender.SendMessageAsync(deleteTaskMessage);
                    }
                    
                    _logger.LogError(e.Message);
                }
              
                await Task.Delay(500, cancellationToken);
            }
        }
    }
}