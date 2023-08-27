using Business.IServices;
using Dtos;
using Entities;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    public class TeamController : Controller
    {

        ITeamService teamService;

        public TeamController(ITeamService _teamService)
        {
            teamService = _teamService;
        }
        [HttpPost]
        [Route("ActivateOverflowTeam")]
        public async Task<BaseResponse<bool>> ActivateOverflowTeam()
        {
            return await teamService.ActivateOverflowTeam();
        }
        [HttpPost]
        [Route("ActivateTeam")]
        async Task<BaseResponse<Team>> ActivateTeam()
        {
            return await teamService.ActivateTeam();
        }
    }
}
