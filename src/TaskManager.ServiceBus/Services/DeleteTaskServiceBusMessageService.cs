using System.Text;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using Polly.Retry;
using TaskManager.Messaging;
using TaskManager.Messaging.Messages;

namespace TaskManager.ServiceBus.Services
{
    public sealed class DeleteTaskServiceBusMessageService : IMessagingService<DeleteTaskMessage>
    {
        private readonly ServiceBusSender _serviceBusSender;
        private readonly AsyncRetryPolicy _retryPolicy;

        public DeleteTaskServiceBusMessageService(
            ServiceBusSender serviceBusSender,
            AsyncRetryPolicy retryPolicy)
        {
            _serviceBusSender = serviceBusSender;
            _retryPolicy = retryPolicy;
        }
        
        public async Task SendMessageAsync(DeleteTaskMessage message)
        {
            await _retryPolicy.ExecuteAsync(async () =>
            {
                var serviceBusMessage = new ServiceBusMessage(
                    Encoding.UTF8.GetBytes(
                        JsonConvert.SerializeObject(message)));
                await _serviceBusSender.SendMessageAsync(serviceBusMessage);
            });
        }
    }
}