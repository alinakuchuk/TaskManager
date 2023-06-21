using System;
using MediatR;

namespace TaskManager.Api.Commands
{
    public sealed class DeleteTaskCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}