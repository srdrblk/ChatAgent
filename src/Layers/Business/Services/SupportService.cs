using Business.IServices;
using Business.Queues;
using Dtos;
using Entities;

namespace Business.Services
{
    public class SupportService : ISupportService
    {
        ChatQueue chatQueue;
        public SupportService(ChatQueue _chatQueue)
        {
            chatQueue = _chatQueue;
        }

        public async Task<bool> CheckCreateSupportIsAvailable()
        {
            return await Task.Run(() => true);
        }
    }
}
