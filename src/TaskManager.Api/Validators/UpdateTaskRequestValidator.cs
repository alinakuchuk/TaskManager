using FluentValidation;
using TaskManager.Api.Command;

namespace TaskManager.Api.Validators
{
    public sealed class UpdateTaskRequestValidator : AbstractValidator<UpdateTaskRequest>
    {
        public UpdateTaskRequestValidator()
        {
            RuleFor(request => request.Id).NotEmpty().WithMessage("Id is required.");
            RuleFor(request => request.Task.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(request => request.Task.DueDateTime).NotEmpty().WithMessage("DueDateTime is required.");
        }
    }
}