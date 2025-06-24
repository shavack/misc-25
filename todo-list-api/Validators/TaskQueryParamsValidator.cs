using System.Data;
using FluentValidation;
using TodoListApi.Application.Dtos;
using TodoListApi.Types;
namespace TodoListApi.Application.Validators;
public class TaskQueryParamsValidator : AbstractValidator<TaskQueryParams>
{
    public TaskQueryParamsValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0).WithMessage("Page must be greater than 0.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 10).WithMessage("PageSize must be between 1 and 110.");

        RuleFor(x => x.SortOrder)
            .Must(value => value == "asc" || value == "desc")
            .When(x => !string.IsNullOrEmpty(x.SortOrder))
            .WithMessage("Sort must be either 'asc' or 'desc'.");

        RuleFor(x => x.SortBy)
            .Must(value => value == "dueDate" || value == "title")
            .When(x => !string.IsNullOrEmpty(x.SortBy))
            .WithMessage("SortBy must be either 'dueDate' or 'title'.");

        RuleFor(x => x.State)
            .Must(value => value == TaskState.Completed || value == TaskState.InProgress || value == TaskState.NotStarted || value == null)
            .When(x => x.State is not null)
            .WithMessage("IsCompleted must be either true or false.");
    }
}