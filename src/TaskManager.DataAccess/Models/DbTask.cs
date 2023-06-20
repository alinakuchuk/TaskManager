using System;

namespace TaskManager.DataAccess.Models
{
    public sealed class DbTask
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }
        
        public string Description { get; set; }

        public bool IsDone { get; set; }
        
        public bool IsDeleted { get; set; }
        
        public DateTime DueDate { get; set; }
        
        public DateTime CreatedDate { get; set; }
        
        public DateTime DeletedDate { get; set; }
    }
}