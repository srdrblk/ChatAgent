using Dtos;
using Entities;

namespace Business.IServices
{
    public interface IChatService
    {
        Task<BaseResponse<string>> CreateChat(SupportDto supportDto, long userId);
        Task AddChat(Chat chat);
        Task<BaseResponse<Message>> AddMessageToChat(long chatId, Message message);
        Task ChatCompleted(long chatId);
        Task CheckDelayOfChats();
 
    }
}
