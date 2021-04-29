using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DasCleverle.DcsExport.Listener
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDcsExportListener(this IServiceCollection services)
        {
            services.AddHostedService<DcsExportListenerService>();
            return services;
        }

        public static IServiceCollection AddDcsExportListener(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHostedService<DcsExportListenerService>();
            services.Configure<ExportListenerOptions>(configuration);

            return services;
        }

        public static IServiceCollection AddDcsExportListener(this IServiceCollection services, string address, int port)
        {
            services.AddHostedService<DcsExportListenerService>();
            services.Configure<ExportListenerOptions>(options =>
            {
                options.Address = address;
                options.Port = port;
            });

            return services;
        }
    }
}

