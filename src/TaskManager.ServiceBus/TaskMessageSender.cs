using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Polly.Retry;
using TaskManager.Messaging;

namespace TaskManager.ServiceBus
{
    public sealed class TaskMessageSender<TMessage> : IMessageSender<TMessage>
    {
        private readonly ServiceBusSender _serviceBusSender;
        private readonly AsyncRetryPolicy _retryPolicy;
        private readonly IMessageSerialization<TMessage> _messageSerialization;

        public TaskMessageSender(
            ServiceBusSender serviceBusSender,
            AsyncRetryPolicy retryPolicy,
            IMessageSerialization<TMessage> messageSerialization)
        {
            _serviceBusSender = serviceBusSender;
            _retryPolicy = retryPolicy;
            _messageSerialization = messageSerialization;
        }
        
        public async Task SendMessageAsync(TMessage message)
        {
            await _retryPolicy.ExecuteAsync(async () =>
            {
                var serviceBusMessage = new ServiceBusMessage(_messageSerialization.Serialize(message));
                await _serviceBusSender.SendMessageAsync(serviceBusMessage);
            });
        }
    }
}