using System.Threading.Tasks;

namespace TaskManager.Messaging
{
    public interface IMessagingService<in TMessage>
    {
        Task SendMessageAsync(TMessage message);
    }
}