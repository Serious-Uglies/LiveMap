using DasCleverle.DcsExport.Client.Abstractions.Popups;
using DasCleverle.DcsExport.Extensibility.Abstractions;
using DasCleverle.DcsExport.State.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace DasCleverle.DcsExport.SampleExtension;

// The extension startup. 
// This is the entry point where we add our custom services
// to the service collection, so that the server knows which parts
// of the app to extend.
public class Startup : IExtensionStartup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<IReducer, SampleReducer>();
        services.AddTransient<IPopupExtender, SamplePopupExtender>();
    }
}
