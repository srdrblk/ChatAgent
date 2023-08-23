using Business.IServices;
using Dtos;
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
     
    }
}
