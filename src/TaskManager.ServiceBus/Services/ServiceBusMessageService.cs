using System.Text;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using TaskManager.Messaging;
using TaskManager.Messaging.Messages;

namespace TaskManager.ServiceBus.Services
{
    public sealed class ServiceBusMessageService : IMessagingService<CreateTaskMessage>
    {
        private readonly ServiceBusSender _serviceBusSender;
        
        public ServiceBusMessageService(ServiceBusSender serviceBusSender)
        {
            _serviceBusSender = serviceBusSender;
        }
        
        public async Task SendMessageAsync(CreateTaskMessage message)
        {
            var serviceBusMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(
                JsonConvert.SerializeObject(message)));
            
            await _serviceBusSender.SendMessageAsync(serviceBusMessage);
        }
    }
}