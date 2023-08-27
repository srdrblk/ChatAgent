using Business.IServices;
using Common.Enums;

namespace Business.Services
{
    public class SupportService : ISupportService
    {

        ITeamService teamService;
        public SupportService(ITeamService _teamService)
        {
            teamService = _teamService;
        }

        public async Task<bool> CheckCreateSupportIsAvailable()
        {
            var availableAgent = await teamService.GetAvailableAgent();
            if (availableAgent == null)
            {
                return true;
            }
            var currentShift = await teamService.GetTeamTypeForCurrentShift();
            if (TeamShiftType.DayShift == currentShift)
            {
                var overflowTeamIsActive = await teamService.CheckTeamIsActiveByTeamShiftType(TeamShiftType.Overflow);
                if (!overflowTeamIsActive)
                {
                    return true;
                }
            }

            return await Task.Run(() => false);
        }
    }
}
