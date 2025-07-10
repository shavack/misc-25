using System;
using FluentValidation;
using TodoListApi.Application.Dtos;
using TodoListApi.Types;

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

            RuleFor(task => task.State)
                .Must(value => value == TaskState.Completed || value == TaskState.InProgress || value == TaskState.NotStarted).WithMessage("IsCompleted must be either Completed, InProgress, or NotStarted.");
            RuleFor(task => task.Description)
                .MaximumLength(100).WithMessage("Description cannot exceed 100 characters.");

            RuleFor(task => task.CompletedAt)
                .NotEmpty().When(task => task.State == TaskState.Completed)
                .WithMessage("CompletedAt must be provided when the task is completed.")
                .Must(date => date == null || date <= DateOnly.FromDateTime(DateTime.Now));
            RuleFor(task => task.CompletedAt)
                .Empty().When(task => task.State != TaskState.Completed)
                .WithMessage("CompletedAt must be empty when the task is not completed.");

            RuleFor(task => task.DueDate)
                .Must(date => date == null || date >= DateOnly.FromDateTime(DateTime.Now))
                .WithMessage("DueDate must be today or in the future.");

            RuleFor(task => task.ProjectId)
                .GreaterThanOrEqualTo(0).WithMessage("ProjectId must be greater than or equal to 0.")
                .WithMessage("ProjectId cannot be negative.");
        }
    }
}