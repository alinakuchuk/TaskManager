using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using TaskManager.Messaging;
using TaskManager.Messaging.Messages;

namespace TaskManager.ServiceBus.Services
{
    public sealed class ServiceBusMessageService : IMessagingService<CreateTaskMessage>
    {
        private readonly IQueueClient _queueClient;
        
        public ServiceBusMessageService(IQueueClient queueClient)
        {
            _queueClient = queueClient;
        }
        
        public async Task SendMessageAsync(CreateTaskMessage message)
        {
            var busMessage = new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message)));

            await _queueClient.SendAsync(busMessage);
        }
    }
}