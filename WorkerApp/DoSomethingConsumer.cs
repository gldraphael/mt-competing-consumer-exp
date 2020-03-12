using MassTransit;
using MessageContracts;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace WorkerApp
{
    class DoSomethingConsumer : IConsumer<DoSomething>
    {
        private readonly ILogger<DoSomethingConsumer> logger;

        public DoSomethingConsumer(ILogger<DoSomethingConsumer> logger)
        {
            this.logger = logger;
        }

        public async Task Consume(ConsumeContext<DoSomething> context)
        {
            logger.LogDebug("Received message: {@DoSomething}", context.Message);
            await Task.Delay(5_000);
            logger.LogInformation("✔ Did something for {@For}", context.Message.For);
        }
    }
}
