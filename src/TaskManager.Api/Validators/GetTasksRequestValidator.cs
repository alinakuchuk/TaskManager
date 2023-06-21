using FluentValidation;
using TaskManager.Api.Query;

namespace TaskManager.Api.Validators
{
    public sealed class GetTasksRequestValidator : AbstractValidator<GetTasksRequest>
    {
        public GetTasksRequestValidator()
        {
            RuleFor(request => request.Offset).NotEmpty().WithMessage("Offset is required.");
            RuleFor(request => request.Limit).NotEmpty().WithMessage("Limit is required.");
        }
    }
}