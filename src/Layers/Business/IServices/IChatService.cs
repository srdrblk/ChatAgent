using Dtos;
using Entities;

namespace Business.IServices
{
    public interface IChatService
    {
        Task<BaseResponse<string>> AddChatToQueue(SupportDto supportDto, long userId);
        Task AddChatToQueue(Chat chat);
        Task<Chat?> GetAndRemoveChatFromQueue();
        Task SetChatToAgent(Chat chat);
        Task<BaseResponse<Message>> AddMessageToChat(long chatId, Message message);
        Task ChatCompleted(long chatId);
        Task CheckDelayOfChats();
        Task<List<Chat>> GetActiveChats();
        Task<List<Chat>> GetActiveChatsByTeamId(long teamId);

    }
}
