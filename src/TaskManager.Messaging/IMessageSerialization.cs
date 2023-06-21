namespace TaskManager.Messaging
{
    public interface IMessageSerialization<TMessage>
    {
        TMessage Deserialize(byte[] messageBody);

        byte[] Serialize(TMessage message);
    }
}