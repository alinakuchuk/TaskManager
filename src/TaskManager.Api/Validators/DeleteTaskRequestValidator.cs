using FluentValidation;
using TaskManager.Api.Command;

namespace TaskManager.Api.Validators
{
    public sealed class DeleteTaskRequestValidator : AbstractValidator<DeleteTaskRequest>
    {
        public DeleteTaskRequestValidator()
        {
            RuleFor(request => request.Id).NotEmpty().WithMessage("Id is required.");
        }
    }
}