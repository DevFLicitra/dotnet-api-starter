using Api.Contracts.Projects;
using FluentValidation;

namespace Api.Validation;

public sealed class ProjectCreateRequestValidator : AbstractValidator<ProjectCreateRequest>
{
     public ProjectCreateRequestValidator()
    {
        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .Must(s => !string.IsNullOrWhiteSpace(s))
            .WithMessage("Name must not be empty or space.")
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .MaximumLength(2000);

    }


}