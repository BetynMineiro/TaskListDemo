using FluentValidation;

namespace Task.Domain.Specification.Interfaces;

public interface ITaskSpecification
{
    void AddRuleNameShouldNotEmpty(AbstractValidator<Entities.Task> validator);
    void AddRuleOwnerShouldNotEmpty(AbstractValidator<Entities.Task> validator);
    void AddRuleTeamShouldNotEmpty(AbstractValidator<Entities.Task> validator);
}

