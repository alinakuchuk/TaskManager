using System;
using MediatR;
using TaskManager.Api.Command;

namespace TaskManager.Api.Commands
{
    public sealed class UpdateTaskCommand : IRequest
    {
        public Guid Id { get; set; }

        public ManagedTask Task { get; set; }
    }
}