using MediatR;
using TaskManager.Api.Command;

namespace TaskManager.Api.Commands
{
    public sealed class CreateTaskCommand : IRequest
    {
        public ManagedTask Task { get; set; }
    }
}