using FluentValidation;
using TodoListApi.Application.Dtos;

namespace TodoListApi.Application.Validators
{
    public class TaskItemValidator : AbstractValidator<TaskItemDto>
    {
        public TaskItemValidator()
        {
            RuleFor(task => task.Title)
                .NotEmpty().WithMessage("Title cannot be empty.")
                .MaximumLength(30).WithMessage("Title cannot exceed 30 characters.")
                .MinimumLength(3).WithMessage("Title must be at least 3 characters long.");

            RuleFor(task => task.IsCompleted)
                .NotNull().WithMessage("IsCompleted cannot be null.")
                .Must(value => value == true || value == false).WithMessage("IsCompleted must be either true or false.");
        }
    }
}