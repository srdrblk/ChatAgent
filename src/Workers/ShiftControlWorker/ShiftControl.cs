using Business.IServices;

namespace ShiftControlWorker
{
    public class ShiftControl : BackgroundService
    {
        private readonly ILogger<ShiftControl> _logger;
        private readonly ITeamService teamService;

        public ShiftControl(ILogger<ShiftControl> logger, ITeamService _teamService)
        {
            _logger = logger;
            teamService = _teamService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Shift Control Worker running at: {time}", DateTimeOffset.Now);
                var haveTeamActiveChats = true;
                while (haveTeamActiveChats)
                {
                    haveTeamActiveChats = await teamService.CloseTeamThatNotActiveIfDoNotHaveActiveChats();
                    if (haveTeamActiveChats)
                    {
                        await teamService.PassiveTheStatusOfTheTeamWaitingForActiveChats();
                    }
                    await Task.Delay(TimeSpan.FromSeconds(8));
                }

                await Task.Delay(TimeSpan.FromHours(8), stoppingToken);
            }
        }
    }
}