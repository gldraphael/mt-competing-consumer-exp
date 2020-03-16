using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AsbWorkerApp
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IQueueClient queueClient;

        public Worker(ILogger<Worker> logger, IQueueClient queueClient)
        {
            _logger = logger;
            this.queueClient = queueClient;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            RegisterOnMessageHandlerAndReceiveMessages();
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await queueClient.CloseAsync();
            await base.StopAsync(cancellationToken);   
        }

        void RegisterOnMessageHandlerAndReceiveMessages()
        {
            // Configure the message handler options in terms of exception handling, number of concurrent messages to deliver, etc.
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                // Maximum number of concurrent calls to the callback ProcessMessagesAsync(), set to 1 for simplicity.
                // Set it according to how many messages the application wants to process in parallel.
                MaxConcurrentCalls = 1,

                // Indicates whether the message pump should automatically complete the messages after returning from user callback.
                // False below indicates the complete operation is handled by the user callback as in ProcessMessagesAsync().
                AutoComplete = false
            };

            // Register the function that processes messages.
            queueClient.RegisterMessageHandler(async (message, token) =>
            {
                await ProcessMessagesAsync(message, token).ConfigureAwait(false);
            }, messageHandlerOptions);
        }

        async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            // Process the message.
            _logger.LogInformation("Received message: SequenceNumber:{@SequenceNumber} Body:{@string}", message.SystemProperties.SequenceNumber, Encoding.UTF8.GetString(message.Body));

            await Task.Delay(5_000);

            // Complete the message so that it is not received again.
            // This can be done only if the queue Client is created in ReceiveMode.PeekLock mode (which is the default).
            await queueClient.CompleteAsync(message.SystemProperties.LockToken);

            // Note: Use the cancellationToken passed as necessary to determine if the queueClient has already been closed.
            // If queueClient has already been closed, you can choose to not call CompleteAsync() or AbandonAsync() etc.
            // to avoid unnecessary exceptions.
        }

        // Use this handler to examine the exceptions received on the message pump.
        Task ExceptionReceivedHandler(ExceptionReceivedEventArgs args)
        {
            _logger.LogError(args.Exception, "Exceptioned: {@ExceptionReceivedContext}", args.ExceptionReceivedContext);
            return Task.CompletedTask;
        }
    }
}
