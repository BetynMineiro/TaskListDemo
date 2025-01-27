using FluentValidation;
using Task.Domain.Specification.Interfaces;
using Task.Domain.Validators.Interfaces;

namespace Task.Domain.Validators;

public class IsValidTaskValidator: AbstractValidator<Entities.Task>,  IIsValidTaskValidator
{
    public IsValidTaskValidator(ITaskSpecification specification)
    {
        specification.AddRuleNameShouldNotEmpty(this);
        specification.AddRuleOwnerShouldNotEmpty(this);
        specification.AddRuleTeamShouldNotEmpty(this);
    }
}