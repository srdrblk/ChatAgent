using Business.IServices;
using Common.Enums;
using Common.Extensions;
using Dtos;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Helper;

namespace Presentation.Controllers
{
    [ApiController]
    [Authorize]
    public class ChatController : Controller
    {
        private readonly IChatService chatService;
        public ChatController(IChatService _chatService)
        {
            chatService = _chatService;
        }
        [HttpPost]
        [Route("SendMessage")]
        [Roles(RoleType.User, RoleType.Agent)]
        public IActionResult SendMessage([FromBody] MessageDto messageDto)
        {
            var chatId = Guid.NewGuid();
            var messageDirection = User.IsInRole(RoleType.User.GetDisplayName()) ? MessageDirection.InComming : MessageDirection.OutGoing;
            var message = new Message() { Text = messageDto.Text, Id = Guid.NewGuid(), Direction = messageDirection };
            chatService.AddMessageToChat(chatId, message);
            return new ObjectResult("test");
        }

        [HttpPost]
        [Route("testagent")]
        [Roles(RoleType.Agent)]
        public IActionResult testagent()
        {
            var t = User.IsInRole(RoleType.Agent.GetDisplayName());
            return Ok("Agent : " + t);
        }
        [HttpPost]
        [Route("testuser")]
        [Roles(RoleType.User)]
        public IActionResult testuser()
        {
            var t = User.IsInRole(RoleType.Agent.GetDisplayName());
            return Ok("user : " + t);
        }
    }
}
