using Microsoft.Extensions.DependencyInjection;

namespace DasCleverle.DcsExport.Extensibility;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddExtensibility(this IServiceCollection services, ExtensionManager extensionManager)
    {
        services.AddSingleton<IExtensionManager>(extensionManager);
        services.AddHostedService<ExtensionStartupMessage>();
        services.AddHostedService<ExtensionScriptInstaller>();

        return services;
    }  
}
