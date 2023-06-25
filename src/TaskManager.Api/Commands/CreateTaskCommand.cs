using MediatR;
using TaskManager.Api.Command;

namespace TaskManager.Api.Commands
{
    public record CreateTaskCommand(CommandTask Task) : IRequest;
}