using System.Threading.Tasks;

namespace TaskManager.Messaging
{
    public interface IMessageSender<in TMessage>
    {
        Task SendMessageAsync(TMessage message);
    }
}