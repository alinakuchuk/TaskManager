using System.Text;
using Newtonsoft.Json;

namespace TaskManager.Messaging
{
    public sealed class JsonMessageSerialization<TMessage> : IMessageSerialization<TMessage>
    {
        public TMessage Deserialize(byte[] messageBody)
        {
            return JsonConvert.DeserializeObject<TMessage>(Encoding.UTF8.GetString(messageBody));
        }

        public byte[] Serialize(TMessage message)
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
        }
    }
}