using Microsoft.Extensions.DependencyInjection;
using Task.Domain.Specification;
using Task.Domain.Specification.Interfaces;
using Task.Domain.Validators;
using Task.Domain.Validators.Interfaces;

namespace Task.Domain;

public static class DomainModule
{
    public static void ConfigureDomainLayer(this IServiceCollection services)
    {
        // Specifications
        services.AddScoped<ITaskSpecification, TaskSpecification>();

        //Validators
        services.AddScoped<IIsValidTaskValidator, IsValidTaskValidator>();
    }
}