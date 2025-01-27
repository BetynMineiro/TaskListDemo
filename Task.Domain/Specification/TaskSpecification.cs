using FluentValidation;
using Task.Domain.Specification.Interfaces;

namespace Task.Domain.Specification;

public class TaskSpecification : ITaskSpecification
{
    private const string FullnameIsRequiredMessage = "Name is required";
    private const string OwnerIsRequiredMessage = "Owner is required";
    private const string TeamIsRequiredMessage = "Team Name is required";

    public void AddRuleNameShouldNotEmpty(AbstractValidator<Entities.Task> validator)
    {
        validator.RuleFor(entity => entity.Name)
            .NotEmpty().WithMessage(FullnameIsRequiredMessage);
    }

    public void AddRuleOwnerShouldNotEmpty(AbstractValidator<Entities.Task> validator)
    {
        validator.RuleFor(entity => entity.Owner)
            .NotEmpty().WithMessage(OwnerIsRequiredMessage);
    }

    public void AddRuleTeamShouldNotEmpty(AbstractValidator<Entities.Task> validator)
    {
        validator.RuleFor(entity => entity.Team)
            .NotEmpty().WithMessage(TeamIsRequiredMessage);
    }
}