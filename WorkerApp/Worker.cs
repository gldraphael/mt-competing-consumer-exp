using System.Threading;
using System.Threading.Tasks;
using GreenPipes;
using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WorkerApp
{
    public class Worker : IHostedService
    {
        private readonly IBusControl busControl;
        private readonly ILogger<Worker> logger;

        public Worker(IBusControl busControl, ILogger<Worker> logger)
        {
            this.busControl = busControl;
            this.logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogDebug("{@ProbeResult}", busControl.GetProbeResult());
            await busControl.StartAsync(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await busControl.StopAsync(cancellationToken);
        }
    }
}
