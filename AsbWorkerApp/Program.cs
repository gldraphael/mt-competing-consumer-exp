using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace AsbWorkerApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
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
                    var connectionString = hostContext.Configuration.GetValue<string>("ASB");
                    var queueName = hostContext.Configuration.GetValue<string>("QueueName");

                    services.AddTransient<IQueueClient>((sp) => new QueueClient(connectionString: connectionString, entityPath: queueName));
                    services.AddHostedService<Worker>();
                });
    }
}
