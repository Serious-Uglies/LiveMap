using System.Text.Json.Serialization;
using DasCleverle.DcsExport.Listener;
using DasCleverle.DcsExport.Listener.Json;
using DasCleverle.DcsExport.LiveMap.Handlers;
using DasCleverle.DcsExport.LiveMap.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DasCleverle.DcsExport.LiveMap
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages()
                .AddRazorRuntimeCompilation()
                .AddJsonOptions(json =>
                {
                    json.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    json.JsonSerializerOptions.Converters.Add(new JsonExportEventConverter());
                });

            services.AddSignalR()
                .AddJsonProtocol(options => 
                {
                    options.PayloadSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    options.PayloadSerializerOptions.Converters.Add(new JsonExportEventConverter());
                });

            services.AddDcsExportListener(Configuration.GetSection("ExportListener"));
            services.AddTransient<IExportEventHandler, LiveMapEventHandler>();

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
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapHub<LiveMapHub>("/hub/livemap");
            });
        }
    }
}
