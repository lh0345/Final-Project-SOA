using FluentValidation;
using TaskFlow.Application.DTOs;

namespace TaskFlow.Application.Validators;

/// <summary>
/// Validates <see cref="TaskUpdateDto"/> ensuring title and description adhere to length constraints.
/// </summary>
public class TaskUpdateValidator : AbstractValidator<TaskUpdateDto>
{
    public TaskUpdateValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.");
    }
}
