using System;
using System.Collections.Generic;
using MediatR;
using TaskManager.Api.Query;

namespace TaskManager.Api.Queries
{
    public sealed class GetTasksQuery : IRequest<IEnumerable<QueryTask>>
    {
        public DateTime? DueData { get; set; }
        
        public bool? IsDone { get; set; }
        
        public int Limit { get; set; }
        
        public int Offset { get; set; }
    }
}