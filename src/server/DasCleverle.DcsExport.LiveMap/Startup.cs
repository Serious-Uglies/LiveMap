using System.Text.Json;
using System.Text.Json.Serialization;
using DasCleverle.DcsExport.Listener;
using DasCleverle.DcsExport.Listener.Json;
using DasCleverle.DcsExport.Listener.Model;
using DasCleverle.DcsExport.LiveMap.Handlers;
using DasCleverle.DcsExport.LiveMap.Hubs;
using DasCleverle.DcsExport.LiveMap.Localization;
using DasCleverle.DcsExport.LiveMap.State;
using DasCleverle.DcsExport.LiveMap.State.Handlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DasCleverle.DcsExport.LiveMap
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }

        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddJsonOptions(json => ConfigureJsonSerializer(json.JsonSerializerOptions));

            services.AddSignalR()
                .AddJsonProtocol(json => ConfigureJsonSerializer(json.PayloadSerializerOptions));

            services.AddSpaStaticFiles(spa =>
            {
                spa.RootPath = "ClientApp/build";
            });

            services.Configure<MapboxOptions>(Configuration);
            services.AddDcsExportListener(Configuration.GetSection("ExportListener"));

            services.AddSingleton<HubExportEventHandler>();
            services.AddTransient<IExportEventHandler>(sp => sp.GetRequiredService<HubExportEventHandler>());
            services.AddHostedService(sp => sp.GetRequiredService<HubExportEventHandler>());

            services.AddSingleton<ILiveState, LiveState>();
            services.AddTransient<IWriteableLiveState>(sp => (IWriteableLiveState)sp.GetService<ILiveState>());

            services.AddTransient<IExportEventHandler<InitPayload>, InitHandler>();
            services.AddTransient<IExportEventHandler<TimePayload>, TimeHandler>();
            services.AddTransient<IExportEventHandler<AddObjectPayload>, AddObjectHandler>();
            services.AddTransient<IExportEventHandler<UpdateObjectPayload>, UpdateObjectHandler>();
            services.AddTransient<IExportEventHandler<RemoveObjectPayload>, RemoveObjectHandler>();
            services.AddTransient<IExportEventHandler<AddAirbasePayload>, AddAirbaseHandler>();
            services.AddTransient<IExportEventHandler<MissionEndPayload>, MissionEndHandler>();

            services.AddSingleton<ILocalizationProvider, JsonFileLocalizationProvider>();
            services.Configure<JsonFileLocalizationProviderOptions>(options =>
            {
                options.BasePath = "lang";
                options.DisableCache = Environment.IsDevelopment();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
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

                endpoints.MapHub<LiveMapHub>("/hub/livemap");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }

        private void ConfigureJsonSerializer(JsonSerializerOptions options)
        {
            options.Converters.Add(new JsonStringEnumConverter());
            options.Converters.Add(new JsonExportEventConverter());
            options.Converters.Add(new JsonResourceCollectionConverter());
        }
    }
}
