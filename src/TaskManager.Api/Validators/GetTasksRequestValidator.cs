using FluentValidation;
using TaskManager.Api.Query;

namespace TaskManager.Api.Validators
{
    public sealed class GetTasksRequestValidator : AbstractValidator<GetTasksRequest>
    {
        public GetTasksRequestValidator()
        {
            RuleFor(request => request.Offset)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Offset should be greater than or equal to '0'.");
            RuleFor(request => request.Limit)
                .GreaterThanOrEqualTo(0)
                .LessThanOrEqualTo(10)
                .WithMessage("Limit should be greater than or equal to '0' and less than or equal to '10'.");
        }
    }
}