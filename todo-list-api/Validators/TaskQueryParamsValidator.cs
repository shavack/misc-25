using FluentValidation;
using TodoListApi.Application.Dtos;
namespace TodoListApi.Application.Validators;
public class TaskQueryParamsValidator : AbstractValidator<TaskQueryParams>
{
    public TaskQueryParamsValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0).WithMessage("Page must be greater than 0.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 10).WithMessage("PageSize must be between 1 and 110.");

        RuleFor(x => x.Sort)
            .Must(value => value == "asc" || value == "desc")
            .When(x => !string.IsNullOrEmpty(x.Sort))
            .WithMessage("Sort must be either 'asc' or 'desc'.");

        RuleFor(x => x.IsCompleted)
            .Must(value => value == true || value == false)
            .When(x => x.IsCompleted.HasValue)
            .WithMessage("IsCompleted must be either true or false.");
    }
}