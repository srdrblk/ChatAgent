using Business.IServices;
using Dtos;
using Entities;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Presentation.Controllers
{
    [ApiController]
    public class SupportController : Controller
    {
        ISupportService supportService;
        IAuthenticationService authenticationService;
        public SupportController(ISupportService _supportService, IAuthenticationService _authenticationService)
        {
            supportService = _supportService;
            authenticationService = _authenticationService;
        }
        [HttpPost]
        [Route("CreateSupport")]
        public async Task<IActionResult> CreateSupport([FromBody] SupportDto supportDto)
        {
            var token = "";
            var isSupportAvailable = await supportService.CheckCreateSupportIsAvailable();
            if (isSupportAvailable)
            {
                var authenticationDto = authenticationService.Authenticate(supportDto?.User?.FullName, Common.Enums.RoleType.User);
            
                await supportService.CreateSupport(supportDto, authenticationDto.UserId);

                return Ok(token);
            }

            return NotFound("Rejected");
        }
    }
}
