using System;

namespace TaskManager.Services.Models
{
    public sealed class DtoTask
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }
        
        public string Description { get; set; }

        public bool IsDone { get; set; }
        
        public DateTime DueDateTime { get; set; }
        
        public DateTime CreatedDateTime { get; set; }
        
        public DateTime DeletedDateTime { get; set; }
    }
}