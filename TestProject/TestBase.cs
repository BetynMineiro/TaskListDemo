using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Task.Api;

namespace TestProject;

public abstract class TestBase
{
    // Attributes
    protected readonly IServiceCollection Services;

    // Properties
    protected ServiceProvider ServiceProvider;

    // Constructor
    protected TestBase()
    {
     Services = new ServiceCollection();

     var configuration = new ConfigurationBuilder()
         .SetBasePath(AppContext.BaseDirectory)
         .AddJsonFile(Path.Combine(AppContext.BaseDirectory, "appsettings.json"))
         .AddEnvironmentVariables()
         .Build();

       ApiModule.ConfigureApiServicesLayer(Services, configuration);

        ConfigureServicesInternal(Services);
        BuildServiceProvider();
    }

    private void BuildServiceProvider()
    {
        ServiceProvider = Services.BuildServiceProvider();
    }

    private void ConfigureServicesInternal(IServiceCollection services)
    {
        ConfigureServices(services);
    }

    protected abstract void ConfigureServices(IServiceCollection services);
}