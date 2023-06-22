using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TaskManager.Messaging;
using TaskManager.Messaging.Messages;
using TaskManager.Services.Interfaces;
using TaskManager.Services.Models;

namespace TaskManager.WorkerService.CommandHandlers
{
    public sealed class UpdateTaskMessageHandler : BackgroundService
    {
        private readonly ILogger<UpdateTaskMessageHandler> _logger;
        private readonly ServiceBusReceiver _serviceBusReceiver;
        private readonly IMessageSerialization<UpdateTaskMessage> _messageSerialization;
        private readonly IMapper _mapper;
        private readonly IServiceProvider _serviceProvider;

        public UpdateTaskMessageHandler(
            ILogger<UpdateTaskMessageHandler> logger,
            ServiceBusReceiver serviceBusReceiver,
            IMessageSerialization<UpdateTaskMessage> messageSerialization,
            IMapper mapper,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceBusReceiver = serviceBusReceiver;
            _messageSerialization = messageSerialization;
            _mapper = mapper;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                UpdateTaskMessage updateTaskMessage = null;
                try
                {
                    var message = await _serviceBusReceiver.ReceiveMessageAsync(cancellationToken: cancellationToken);
                    if (message != null)
                    {
                        updateTaskMessage = _messageSerialization.Deserialize(message.Body.ToArray());
                        var dtoTask = _mapper.Map<DtoTask>(updateTaskMessage);
                        using (var scope = _serviceProvider.CreateScope())
                        {
                            var service = scope.ServiceProvider.GetRequiredService<ICommandTaskService>();
                            await service.UpdateTaskAsync(updateTaskMessage.Id, dtoTask, cancellationToken);
                        }
                    
                        await _serviceBusReceiver.CompleteMessageAsync(message, cancellationToken);
                    }
                    
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }
                catch (Exception e)
                {
                    if (updateTaskMessage != null)
                    {
                        using var scope = _serviceProvider.CreateScope();
                        var sender = scope.ServiceProvider.GetRequiredService<IMessageSender<UpdateTaskMessage>>();
                        await sender.SendMessageAsync(updateTaskMessage);
                    }
                    
                    _logger.LogError(e.Message);
                }
              
                await Task.Delay(500, cancellationToken);
            }
        }
    }
}