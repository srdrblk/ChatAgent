using Business.IServices;
using Microsoft.Extensions.DependencyInjection;

namespace ShiftControlWorker
{
    public class ShiftControl : BackgroundService
    {
        private readonly ILogger<ShiftControl> _logger;
        private readonly IServiceProvider serviceProvider;

        public ShiftControl(ILogger<ShiftControl> logger, IServiceProvider _serviceProvider)
        {
            _logger = logger;
            serviceProvider = _serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                _logger.LogInformation("Shift Control Worker running at: {time}", DateTimeOffset.Now);
                var haveTeamActiveChats = true;
                using (IServiceScope scope = serviceProvider.CreateScope())
                {
                    ITeamService teamService =
                        scope.ServiceProvider.GetRequiredService<ITeamService>();

                    while (haveTeamActiveChats)
                    {
                        haveTeamActiveChats = await teamService.CloseTeamThatNotActiveIfDoNotHaveActiveChats();
                        if (haveTeamActiveChats)
                        {
                            await teamService.PassiveTheStatusOfTheTeamWaitingForActiveChats();
                        }
                        await Task.Delay(TimeSpan.FromSeconds(8));
                    }
                }
            
             

                await Task.Delay(TimeSpan.FromHours(8), stoppingToken);
            }
        }
    }
}