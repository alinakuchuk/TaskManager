namespace TaskManager.Contracts.Models
{
    public sealed class ServiceBusSettings
    {
        public string ConnectionString { get; set; }
        
        public string CreateTaskQueueName { get; set; }
        
        public string DeleteTaskQueueName { get; set; }
        
        public string UpdateTaskQueueName { get; set; }
        
        public string ErrorCreateTaskQueueName { get; set; }
        
        public string ErrorDeleteTaskQueueName { get; set; }
        
        public string ErrorUpdateTaskQueueName { get; set; }
    }
}