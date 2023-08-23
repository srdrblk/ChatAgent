using Dtos;
using Entities;

namespace Business.IServices
{
    public interface ITeamService
    {
        Task<BaseResponse<bool>> ActivateTeam();
        Task<BaseResponse<bool>> ActivateOverflowTeam();
        Task<Agent> GetAvailableAgent();
    }
}
