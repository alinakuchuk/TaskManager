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
    public sealed class CreateTaskMessageHandler : BackgroundService
    {
        private readonly ILogger<CreateTaskMessageHandler> _logger;
        private readonly ServiceBusReceiver _serviceBusReceiver;
        private readonly IMessageSerialization<CreateTaskMessage> _messageSerialization;
        private readonly IMapper _mapper;
        private readonly IServiceProvider _serviceProvider;

        public CreateTaskMessageHandler(
            ILogger<CreateTaskMessageHandler> logger,
            ServiceBusReceiver serviceBusReceiver,
            IMessageSerialization<CreateTaskMessage> messageSerialization,
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
                CreateTaskMessage createTaskMessage = null;
                try
                {
                    var message = await _serviceBusReceiver.ReceiveMessageAsync(cancellationToken: cancellationToken);
                    if (message != null)
                    {
                        createTaskMessage = _messageSerialization.Deserialize(message.Body.ToArray());
                        var dtoTask = _mapper.Map<DtoTask>(createTaskMessage);
                        using (var scope = _serviceProvider.CreateScope())
                        {
                            var service = scope.ServiceProvider.GetRequiredService<ICommandTaskService>();
                            await service.CreateTaskAsync(dtoTask, cancellationToken);
                        }

                        await _serviceBusReceiver.CompleteMessageAsync(message, cancellationToken);
                    }
                    
                    _logger.LogInformation("Worker running at: {time}", DateTime.UtcNow);
                }
                catch (Exception ex)
                {
                    if (createTaskMessage != null)
                    {
                        using var scope = _serviceProvider.CreateScope();
                        var sender = scope.ServiceProvider.GetRequiredService<IMessageSender<CreateTaskMessage>>();
                        await sender.SendMessageAsync(createTaskMessage);
                    }
                    
                    _logger.LogError(ex.Message);
                }
              
                await Task.Delay(500, cancellationToken);
            }
        }
    }
}