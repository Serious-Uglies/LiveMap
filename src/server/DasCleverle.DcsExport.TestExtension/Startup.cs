using DasCleverle.DcsExport.Extensibility.Abstractions;
using DasCleverle.DcsExport.State.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace DasCleverle.DcsExport.TestExtension;

public class Startup : IExtensionStartup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<IReducer, TestReducer>();
    }
}
