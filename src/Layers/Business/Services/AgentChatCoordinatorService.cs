using Business.IServices;
using Business.Queues;
using Core.Context;

namespace Business.Services
{
    public class AgentChatCoordinatorService: IAgentChatCoordinatorService
    {

        ChatQueue chatQueue;
        private ITeamService teamService;
        private IChatHubService chatHubService;
        private IChatService chatService;
        public AgentChatCoordinatorService(ChatQueue _chatQueue, AgentContext _context, ITeamService _teamService, IChatHubService _chatHubService, IChatService _chatService)
        {
            chatQueue = _chatQueue;
            teamService = _teamService;
            chatHubService = _chatHubService;
            chatService = _chatService;
        }
        public async Task CoordinateChats()
        {
            var availableAgent = await teamService.GetAvailableAgent();
            if (availableAgent != null)
            {
                var chat = await chatQueue.GetAndRemoveChatFromQueue();
                if (chat!=null)
                {
                    chat.Agent = availableAgent;
                    chat.AgentId = chat.AgentId;
                    await chatService.SetChatToAgent(chat);
                    await chatHubService.ChatCreated(chat.User.Id, chat.Agent.Id, chat.Id);
                }
            }
        }
    }
}
