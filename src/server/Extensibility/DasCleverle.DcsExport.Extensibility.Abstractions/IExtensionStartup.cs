using Microsoft.Extensions.DependencyInjection;

namespace DasCleverle.DcsExport.Extensibility.Abstractions;

public interface IExtensionStartup
{
    void ConfigureServices(IServiceCollection services);
}