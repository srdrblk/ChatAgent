using Business.IServices;
using Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    public class SupportController : Controller
    {
        ISupportService supportService;
        IChatService chatService;
        IAuthenticationService authenticationService;
        public SupportController(ISupportService _supportService, IAuthenticationService _authenticationService, IChatService _chatService)
        {
            supportService = _supportService;
            authenticationService = _authenticationService;
            chatService = _chatService;

        }
        [HttpPost]
        [Route("CreateSupport")]
        public async Task<IActionResult> CreateSupport([FromBody] SupportDto supportDto)
        {
            var isSupportAvailable = await supportService.CheckCreateSupportIsAvailable();
            if (isSupportAvailable)
            {
                var authenticationDto = authenticationService.Authenticate(supportDto?.User?.FullName, Common.Enums.RoleType.User);
            
                await chatService.CreateChat(supportDto, authenticationDto.UserId);

                return Ok(authenticationDto.Token);
            }

            return NotFound("Rejected");
        }
    }
}
