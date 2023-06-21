namespace TaskManager.Contracts.Models
{
    public sealed class CosmosDbSettings
    {
        public string ConnectionString { get; set; }
        
        public string PrimaryKey { get; set; }
        
        public string DatabaseName { get; set; }
        
        public string ContainerName { get; set; }
    }
}