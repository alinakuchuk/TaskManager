using FluentValidation;
using TaskManager.Api.Query;

namespace TaskManager.Api.Validators
{
    public sealed class GetTaskRequestValidator : AbstractValidator<GetTaskRequest>
    {
        public GetTaskRequestValidator()
        {
            RuleFor(request => request.Id).NotEmpty().WithMessage("Id is required.");
        }
    }
}