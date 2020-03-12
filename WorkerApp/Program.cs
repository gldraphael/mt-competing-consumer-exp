using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace WorkerApp
{
    public class Program
    {
        public static void Main(string[] args) =>
            CreateHostBuilder(args).Build().Run();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostcontext, config) =>
                {
                    if(hostcontext.HostingEnvironment.IsDevelopment())
                    {
                        config.AddUserSecrets<Program>();
                    }
                })
                .ConfigureLogging((hostContext, logging) =>
                {
                    logging.ClearProviders();
                    var logger = new LoggerConfiguration()
                                    .ReadFrom.Configuration(hostContext.Configuration)
                                    .CreateLogger(); ;
                    logging.AddSerilog(logger);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.SetupMassTransit(hostContext.Configuration);
                    services.AddHostedService<Worker>();
                });
    }
}
