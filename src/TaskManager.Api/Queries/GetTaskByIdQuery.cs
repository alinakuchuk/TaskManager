using System;
using MediatR;
using TaskManager.Api.Query;

namespace TaskManager.Api.Queries
{
    public sealed class GetTaskByIdQuery : IRequest<QueryTask>
    {
        public Guid Id { get; set; }
    }
}