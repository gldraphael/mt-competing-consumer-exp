using MassTransit;
using MassTransit.Azure.ServiceBus.Core;
using MessageContracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WorkerApp
{
    internal static class MassTransitServiceCollectionExtensions
    {
        const int PREFETCH_COUNT = 0; // should turn Prefetch off on ASB

        internal static void SetupMassTransit(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<DoSomethingConsumer>();

            services.AddMassTransit(x =>
            {
                x.AddBus(provider => Bus.Factory.CreateUsingAzureServiceBus(bc =>
                {
                    bc.Host(configuration.GetSection("ASB").Get<string>());
                    bc.PrefetchCount = PREFETCH_COUNT;

                    bc.ReceiveEndpoint(Constants.QueueName, re =>
                    {
                        re.Consumer<DoSomethingConsumer>(provider, c =>
                        {
                            c.UseConcurrentMessageLimit(1);
                        });
                    });
                }));
            });
        }
    }
}
