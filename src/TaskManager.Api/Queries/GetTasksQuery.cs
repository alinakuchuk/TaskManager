using System;
using System.Collections.Generic;
using MediatR;
using TaskManager.Api.Query;

namespace TaskManager.Api.Queries
{
    public record GetTasksQuery(DateTime? DueDateTime, bool? IsDone, int Limit, int Offset)
        : IRequest<IEnumerable<QueryTask>>;
}