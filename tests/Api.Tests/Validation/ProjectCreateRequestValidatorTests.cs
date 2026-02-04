using Api.Contracts.Projects;
using Api.Validation;
using FluentValidation.TestHelper;
using Xunit;

namespace Api.Tests.Validation;

public sealed class ProjectCreateRequestValidatorTests
{
    private readonly ProjectCreateRequestValidator _validator = new();

    [Fact]
    public void Name_is_required()
    {
        var result = _validator.TestValidate(new ProjectCreateRequest("", null));
        result.ShouldHaveValidationErrorFor(x => x.Name);

    }

    [Fact]
    public void Name_cannot_be_whitespace()
    {
        var result = _validator.TestValidate(new ProjectCreateRequest("   ", null));
        result.ShouldHaveValidationErrorFor(x => x.Name); 
    }

    [Fact]
    public void Name_max_length_200()
    {
        var name = new string('a', 201);
        var result = _validator.TestValidate(new ProjectCreateRequest(name, null));
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }
}