using System;

namespace TaskManager.Services.Models
{
    public sealed class DtoGetTasksQueryParameters
    {
        public DateTime? DueDate { get; set; }
        
        public bool? IsDone { get; set; }

        public bool? IsDeleted { get; set; }
        
        public int Offset { get; set; }
        
        public int Limit { get; set; }
    }
}