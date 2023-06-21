namespace TaskManager.Infrastructure.Models
{
    public sealed class CosmosDbSettings
    {
        public string EndpointUri { get; set; }
        
        public string PrimaryKey { get; set; }
        
        public string DatabaseName { get; set; }
        
        public string ContainerName { get; set; }
    }
}