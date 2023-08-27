using Common.Enums;
using Dtos;
using Entities;

namespace Business.IServices
{
    public interface ITeamService
    {
        Task<TeamShiftType> GetTeamTypeForCurrentShift();
        Task<BaseResponse<Team>> ActivateTeam();
        Task<BaseResponse<bool>> ActivateOverflowTeam();
        Task<BaseResponse<bool>> DeActivateOverflowTeam();
        Task<bool> GetAgentChatAvailability(Agent agent);
        Task<List<Agent>> GetActiveTeamAgents();
        Task<List<Agent>> GetOverflowTeamAgents();
        Task<Agent?> GetAvailableAgent();
        Task<List<Team>> GetTeams();
        Task<bool> CheckTeamIsActiveByTeamShiftType(TeamShiftType teamShiftType);
        Task<bool> CloseTeamThatNotActiveIfDoNotHaveActiveChats();
        Task<bool> PassiveTheStatusOfTheTeamWaitingForActiveChats();
    }
}
