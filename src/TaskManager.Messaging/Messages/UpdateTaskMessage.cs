using System;

namespace TaskManager.Messaging.Messages
{
    public sealed class UpdateTaskMessage
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        
        public string Description { get; set; }

        public DateTime DueDateTime { get; set; }

        public bool IsDone { get; set; }
    }
}