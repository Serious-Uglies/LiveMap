using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using DasCleverle.DcsExport.Client.Abstractions.Layers;
using DasCleverle.DcsExport.Client.Abstractions.Popups;
using DasCleverle.DcsExport.Client.Icons;
using DasCleverle.DcsExport.Extensibility;
using DasCleverle.DcsExport.Listener;
using DasCleverle.DcsExport.Listener.Json;
using DasCleverle.DcsExport.LiveMap;
using DasCleverle.DcsExport.LiveMap.Caching;
using DasCleverle.DcsExport.LiveMap.Client;
using DasCleverle.DcsExport.LiveMap.Handlers;
using DasCleverle.DcsExport.LiveMap.Hubs;
using DasCleverle.DcsExport.LiveMap.Localization;
using DasCleverle.DcsExport.State;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;

var builder = WebApplication.CreateBuilder(args);

var extensionManager = new ExtensionManager();
var extensionLoader = new ExtensionLoader(extensionManager);

extensionLoader.LoadExtensions(builder.Environment.ContentRootPath + "/extensions", builder.Services);

builder.Services.AddExtensibility(extensionManager);

builder.Services.AddControllers()
    .AddJsonOptions(json => ConfigureJsonSerializer(json.JsonSerializerOptions));

builder.Services.AddSignalR()
    .AddJsonProtocol(json => ConfigureJsonSerializer(json.PayloadSerializerOptions));

builder.Services.AddSpaStaticFiles(spa =>
{
    spa.RootPath = "ClientApp/build";
});

builder.Services.AddMemoryCache();
builder.Services.AddSingleton<ICache, MemoryCache>();

builder.Services.Configure<MapboxOptions>(builder.Configuration);
builder.Services.Configure<ExtensionOptions>(builder.Configuration.GetSection("Extensions"));

builder.Services.AddTcpExportListener(builder.Configuration.GetSection("ExportListener"));
builder.Services.AddJsonMessageParser();
builder.Services.AddLiveState();

builder.Services.AddHostedService<LiveStateHubService>();

builder.Services.AddSingleton<IconTemplateMapManager>();
builder.Services.AddSingleton<IIconGenerator, IconGenerator>();

builder.Services.AddSingleton<ILayerRegistry, LayerRegistry>();
builder.Services.AddTransient<ILayerProvider, DefaultLayerProvider>();

builder.Services.AddSingleton<IPopupRegistry, PopupRegistry>();
builder.Services.AddTransient<IPopupProvider, ObjectPopupProvider>();
builder.Services.AddTransient<IPopupProvider, AirbasePopupProvider>();

builder.Services.AddSingleton<ILocalizationProvider, JsonFileLocalizationProvider>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseHttpsRedirection();
}

app.UseStaticFiles();
app.UseSpaStaticFiles();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<LiveStateHub>("/hub/state");
});

app.UseSpa(spa =>
{
    spa.Options.SourcePath = "ClientApp";

    if (app.Environment.IsDevelopment())
    {
        if (Debugger.IsAttached)
        {
            spa.UseProxyToSpaDevelopmentServer("http://localhost:3000");
        }
        else
        {
            spa.UseReactDevelopmentServer(npmScript: "start");
        }
    }
});

app.Run();

void ConfigureJsonSerializer(JsonSerializerOptions options)
{
    options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;

    options.Converters.Add(new JsonStringEnumConverter());
    options.Converters.Add(new JsonResourceCollectionConverter());
}