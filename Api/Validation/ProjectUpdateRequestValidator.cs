using Api.Contracts.Projects;
using FluentValidation;

namespace Api.Validation;

public sealed class ProjectUpdateRequestValidator : AbstractValidator<ProjectUpdateRequest>
{
    public ProjectUpdateRequestValidator()
    {
        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .Must(s => !string.IsNullOrWhiteSpace(s))
            .WithMessage("Name must not be empty or with space")
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .MaximumLength(2000);

    }


}