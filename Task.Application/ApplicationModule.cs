using Microsoft.Extensions.DependencyInjection;
using Task.Application.ApplicationServices.NotificationService;

namespace Task.Application;

public static class ApplicationModule
{
    public static void ConfigureApplicationLayer(this IServiceCollection services)
    {
        //Notification
        services.AddScoped<NotificationServiceContext>();
        
    }
}