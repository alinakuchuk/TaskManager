using System;
using MediatR;

namespace TaskManager.Api.Commands
{
    public record DeleteTaskCommand(Guid Id) : IRequest;
}