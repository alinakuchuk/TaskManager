using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using TaskManager.Messaging;

namespace TaskManager.ServiceBus
{
    public sealed class TaskMessageSender<TMessage> : IMessageSender<TMessage>
    {
        private readonly ServiceBusSender _serviceBusSender;
        private readonly AsyncRetryPolicy _retryPolicy;
        private readonly ILogger<TaskMessageSender<TMessage>> _logger;
        private readonly IMessageSerialization<TMessage> _messageSerialization;
        private readonly string _queueName;

        public TaskMessageSender(
            ServiceBusClient serviceBusClient,
            string queueName,
            ILogger<TaskMessageSender<TMessage>> logger,
            IMessageSerialization<TMessage> messageSerialization)
        {
            _serviceBusSender = serviceBusClient.CreateSender(queueName);
            _logger = logger;
            _messageSerialization = messageSerialization;
            _queueName = queueName;
            _retryPolicy = Policy
                .Handle<ServiceBusException>()
                .Or<TimeoutException>()
                .WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    onRetry: (exception, retryAttempt) 
                        => _logger.LogWarning($"Retrying due to exception: {exception.Message}. Retry attempt {retryAttempt}."));
        }
        
        public async Task SendMessageAsync(TMessage message)
        {
            await _retryPolicy.ExecuteAsync(async () =>
            {
                var serviceBusMessage = new ServiceBusMessage(_messageSerialization.Serialize(message));
                await _serviceBusSender.SendMessageAsync(serviceBusMessage);
                _logger.LogInformation(
                    "'{Message}' has been sent to '{queue}' at {time}.", 
                    typeof(TMessage).Name,
                    _queueName,
                    DateTime.UtcNow);
            });
        }
    }
}