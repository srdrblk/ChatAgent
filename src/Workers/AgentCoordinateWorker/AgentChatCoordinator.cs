using Business.IServices;

namespace AgentCoordinateWorker
{
    public class AgentChatCoordinator : BackgroundService
    {
        private readonly ILogger<AgentChatCoordinator> _logger;

        private readonly IServiceProvider serviceProvider;
        public AgentChatCoordinator(ILogger<AgentChatCoordinator> logger, IServiceProvider _serviceProvider)
        {
            _logger = logger;

            serviceProvider = _serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Agent Chat Coordinator running at: {time}", DateTimeOffset.Now);

                using (IServiceScope scope = serviceProvider.CreateScope())
                {
                    IAgentChatCoordinatorService coordinatorService =
                        scope.ServiceProvider.GetRequiredService<IAgentChatCoordinatorService>();

                    await coordinatorService.CoordinateChats();
                }
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}