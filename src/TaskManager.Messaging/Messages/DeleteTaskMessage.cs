using System;

namespace TaskManager.Messaging.Messages
{
    public sealed class DeleteTaskMessage
    {
        public Guid Id { get; set; }
    }
}