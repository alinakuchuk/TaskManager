using FluentValidation;
using TaskManager.Api.Command;

namespace TaskManager.Api.Validators
{
    public sealed class CreateTaskRequestValidator : AbstractValidator<CreateTaskRequest>
    {
        public CreateTaskRequestValidator()
        {
            RuleFor(request => request.Task.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(request => request.Task.DueDateTime).NotEmpty().WithMessage("DueDateTime is required.");
        }
    }
}