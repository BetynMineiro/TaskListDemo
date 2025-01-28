using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Task.Api.Middleware;
using Task.Application;
using Task.CrossCutting;
using Task.CrossCutting.Bus;
using Task.CrossCutting.Configurations;
using Task.Domain;
using Task.MongoDbAdpter;

namespace Task.Api;

public static class ApiModule
{
    public static void ConfigureFilters(this MvcOptions options)
    {
        options.Filters.Add<NotificationServiceMiddleware>();
        options.Filters.Add<AcceptHeaderMiddleware>();
        options.Filters.Add(new ProducesAttribute("application/json"));
    }
    private static void ConfigureAppSettingsApi(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<AppSettingsConfig>(configuration.GetSection("TASK"));
        services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<AppSettingsConfig>>().Value);
    }
    private static void ConfigureMessageHandlerLayer(this IServiceCollection services)
    {
        var assemblyCollection = new List<Assembly>
        {
            typeof(ApplicationModule).Assembly,
            typeof(MongoDbAdapterModule).Assembly,
        };
        services.ConfigureMessageHandler(assemblyCollection);
    }
    public static void ConfigureApiServicesLayer(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IBus, Bus>();
        services.ConfigureAppSettingsApi(configuration);

        //MongoDB Service
        services.ConfigureMongoAdapterLayer(configuration);
        
        services.ConfigureDomainLayer();
        services.ConfigureApplicationLayer();
         
        services.ConfigureLogging();
        services.ConfigureMessageHandlerLayer();
    }
}