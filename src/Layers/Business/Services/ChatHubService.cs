using Business.Hubs;
using Business.IServices;
using Microsoft.AspNetCore.SignalR;

namespace Business.Services
{
    public class ChatHubService : IChatHubService
    {
        private readonly IHubContext<ChatHub> hubContext;
        public ChatHubService(IHubContext<ChatHub> _hubContext)
        {
            hubContext = _hubContext;
        }
        public async Task ChatCompleted(long userId, long agentId, long chatId)
        {
            await hubContext.Clients.Clients(new List<string>() { userId.ToString(), agentId.ToString() }).SendAsync("ChatCompleted", chatId);
        }
        public async Task ChatCreated(long userId, long agentId, long chatId)
        {
            await hubContext.Clients.Clients(new List<string>() { userId.ToString(), agentId.ToString() }).SendAsync("ChatCreated", chatId);
        }
    }
}
