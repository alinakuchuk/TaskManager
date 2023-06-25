using System;
using MediatR;
using TaskManager.Api.Query;

namespace TaskManager.Api.Queries
{
    public record GetTaskByIdQuery(Guid Id) : IRequest<QueryTask>;
}