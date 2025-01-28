using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Task.CrossCutting;
using Task.Domain.Repositories;
using Task.MongoDbAdpter.Context;
using Task.MongoDbAdpter.Repository;

namespace Task.MongoDbAdpter;

public static class MongoDbAdapterModule
{
    public static void ConfigureMongoAdapterLayer(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<IMongoClient>(c =>
        {
            var appSettingsConfig = configuration.GetAppSettingsApiConfig();

            return MongoDbContext.BuildMongoConnection(appSettingsConfig.MongoConfig.Connection,
                appSettingsConfig.MongoConfig.Database);
        });

        services.AddScoped(c => c.GetService<IMongoClient>().StartSession());

        services.AddScoped<ITaskRepository, TaskRepository>();
        
    }
}