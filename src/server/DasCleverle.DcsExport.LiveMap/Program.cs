using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace DasCleverle.DcsExport.LiveMap
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(kestrel =>
                    {
                        kestrel.ListenAnyIP(5000);
                        kestrel.ListenAnyIP(5001, o => o.UseHttps());
                    });
                    webBuilder.UseStartup<Startup>();
                });
    }
}
