using DasCleverle.DcsExport.Listener.Abstractions;
using DasCleverle.DcsExport.State.Abstractions;
using DasCleverle.DcsExport.State.Reducers;
using Microsoft.Extensions.DependencyInjection;

namespace DasCleverle.DcsExport.State;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLiveState(this IServiceCollection services)
    {
        services.AddTransient<IExportEventHandler, LiveStateEventHandler>();
        services.AddSingleton<ILiveStateStore, LiveStateStore>();

        services.AddTransient<IReducer, AddAirbaseReducer>();
        services.AddTransient<IReducer, AddObjectReducer>();
        services.AddTransient<IReducer, InitReducer>();
        services.AddTransient<IReducer, MissionEndReducer>();
        services.AddTransient<IReducer, RemoveObjectReducer>();
        services.AddTransient<IReducer, TimeReducer>();
        services.AddTransient<IReducer, UpdateObjectReducer>();

        return services;
    }
}