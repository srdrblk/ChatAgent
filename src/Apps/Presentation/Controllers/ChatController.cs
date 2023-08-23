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
            var messageDirection = User.IsInRole(RoleType.User.GetDisplayName()) ? MessageDirection.InComming : MessageDirection.OutGoing;
            var message = new Message() { Text = messageDto.Text, Direction = messageDirection, ChatId = messageDto.ChatId };
            chatService.AddMessageToChat(messageDto.ChatId, message);
            return new ObjectResult("test");
        }

    }
}
