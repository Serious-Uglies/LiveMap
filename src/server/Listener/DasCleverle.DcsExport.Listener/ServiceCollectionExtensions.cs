using DasCleverle.DcsExport.Listener.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DasCleverle.DcsExport.Listener;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTcpExportListener(this IServiceCollection services)
    {
        services.AddHostedService<TcpExportListenerService>();
        services.AddTransient<IExportEventHandler, GenericExportEventHandlerPropagator>();

        return services;
    }

    public static IServiceCollection AddTcpExportListener(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTcpExportListener();
        services.Configure<TcpListenerOptions>(configuration);

        return services;
    }

    public static IServiceCollection AddTcpExportListener(this IServiceCollection services, string address, int port)
    {
        services.AddTcpExportListener();
        services.Configure<TcpListenerOptions>(options =>
        {
            options.Address = address;
            options.Port = port;
        });

        return services;
    }
}

