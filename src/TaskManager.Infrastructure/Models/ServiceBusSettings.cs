namespace TaskManager.Infrastructure.Models
{
    public sealed class ServiceBusSettings
    {
        public string ConnectionString { get; set; }
        
        public string CreateTaskQueueName { get; set; }
        
        public string DeleteTaskQueueName { get; set; }
        
        public string UpdateTaskQueueName { get; set; }
    }
}