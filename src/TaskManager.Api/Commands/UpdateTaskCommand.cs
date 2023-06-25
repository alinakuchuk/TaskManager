using System;
using MediatR;
using TaskManager.Api.Command;

namespace TaskManager.Api.Commands
{
    public record UpdateTaskCommand(Guid Id, CommandTask Task) : IRequest;
}