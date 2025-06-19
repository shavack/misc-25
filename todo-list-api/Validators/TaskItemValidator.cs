using System;
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
            RuleFor(task => task.Description)
                .MaximumLength(100).WithMessage("Description cannot exceed 100 characters.");

            RuleFor(task => task.CompletedAt)
                .NotEmpty().When(task => task.IsCompleted)
                .WithMessage("CompletedAt must be provided when the task is completed.")
                .Must(date => date == null || date <= DateTime.Now);
            RuleFor(task => task.CompletedAt)
                .Empty().When(task => !task.IsCompleted)
                .WithMessage("CompletedAt must be empty when the task is not completed.");
        }
    }
}