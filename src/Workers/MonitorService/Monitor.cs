using Business.IServices;

namespace MonitorService
{
    public class Monitor : BackgroundService
    {
        private readonly ILogger<Monitor> _logger;
        private readonly IServiceProvider serviceProvider;
        public Monitor(ILogger<Monitor> logger, IServiceProvider _serviceProvider)
        {
            _logger = logger;
            serviceProvider = _serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Monitor Service running at: {time}", DateTimeOffset.Now);
     
                using (IServiceScope scope = serviceProvider.CreateScope())
                {
                    IChatService chatService =
                        scope.ServiceProvider.GetRequiredService<IChatService>();

                    await chatService.CheckDelayOfChats();
                }
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}