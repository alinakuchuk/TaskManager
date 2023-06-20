using System;

namespace TaskManager.DataAccess.Models
{
    public sealed class DbGetTasksQueryParameters
    {
        public DateTime? DueDate { get; set; }
        
        public bool? IsDone { get; set; }

        public bool? IsDeleted { get; set; }
        
        public int Offset { get; set; }
        
        public int Limit { get; set; }
    }
}