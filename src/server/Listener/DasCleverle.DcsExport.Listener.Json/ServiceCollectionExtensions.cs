using Microsoft.Extensions.DependencyInjection;

namespace DasCleverle.DcsExport.Listener.Json;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddJsonMessageParser(this IServiceCollection services)
    {
        services.AddTransient<IMessageParser, JsonMessageParser>();

        return services;
    }
}