using DasCleverle.DcsExport.Listener.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DasCleverle.DcsExport.Listener
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDcsExportListener(this IServiceCollection services)
        {
            services.AddHostedService<DcsExportListenerService>();
            services.AddTransient<IExportMessageHandler, JsonExportMessageHandler>();
            return services;
        }

        public static IServiceCollection AddDcsExportListener(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDcsExportListener();
            services.Configure<ExportListenerOptions>(configuration);

            return services;
        }

        public static IServiceCollection AddDcsExportListener(this IServiceCollection services, string address, int port)
        {
            services.AddDcsExportListener();
            services.Configure<ExportListenerOptions>(options =>
            {
                options.Address = address;
                options.Port = port;
            });

            return services;
        }
    }
}

