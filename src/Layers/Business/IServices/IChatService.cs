using Dtos;
using Entities;

namespace Business.IServices
{
    public interface IChatService
    {
        Task AddChat(Chat chat);
        Task<BaseResponse<Message>> AddMessageToChat(Guid chatId, Message message);
        Task ChatCompleted(Guid chatId);
        Task CheckDelayOfChats();

    }
}
