using Bogus;
using MassTransit;
using MassTransit.Azure.ServiceBus.Core;
using MessageContracts;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
            var faker = new Faker();
            var config = new ConfigurationBuilder().AddEnvironmentVariables().AddUserSecrets<Program>().Build();
            var bus = Bus.Factory.CreateUsingAzureServiceBus(sbc => _ = sbc.Host(config.GetValue<string>("ASB")));
            var queueName = config.GetValue<string>("QueueName");

            await bus.StartAsync();
            try
            {
                for (int i = 0; i < 8; i++)
                {
                    var command = new DoSomething(@for: faker.Name.FullName());
                    var endpoint = await bus.GetSendEndpoint(new Uri(bus.Address, queueName));
                    await endpoint.Send(command);
                    Log.Information("Sent command {@DoSomething}", command);
                }
            }
            finally
            {
                await bus.StopAsync();
            }
        }
    }
}
