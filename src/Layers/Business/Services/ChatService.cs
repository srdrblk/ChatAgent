using Business.IServices;
using Common.Enums;
using Dtos;
using Entities;

namespace Business.Services
{
    public class ChatService : IChatService
    {
        private List<Chat> Chats = new List<Chat>();
        private int WaitingDurationLimit = 3;
        public ChatService()
        {

        }
        public async Task AddChat(Chat chat)
        {
            chat.Id = Guid.NewGuid();
            Chats.Add(chat);
        }
        public async Task<BaseResponse<Message>> AddMessageToChat(Guid chatId, Message message)
        {
            var response = new BaseResponse<Message>();
            var chat = Chats.FirstOrDefault(c => c.Id == chatId);
            if (chat == null)
            {
                response.Statu = ResponseStatu.Error;
                response.Message = "Chat is not exist!";
                return response;
            }
            chat.Messages.Enqueue(message);
            response.Statu = ResponseStatu.Success;
            return response;

        }
        public async Task ChatCompleted(Guid chatId)
        {
            var chat = Chats.FirstOrDefault(c => c.Id == chatId);
            Chats.Remove(chat);
            chat.Statu = ChatStatu.SupportCompleted;
            //log chat
        }
        private async Task ChatClosedDueToWaiting(List<Guid> chatIdsWillCloseDueToWaiting)
        {
            await Task.Run(() =>
                   action(chatIdsWillCloseDueToWaiting)
            );

            void action(List<Guid> chatIds)
            {
                var chatsThatWillNotBeDeleted = Chats.Where(c => !chatIds.Contains(c.Id)).ToList();
                Chats = chatsThatWillNotBeDeleted;
            };

        }
        public async Task CheckDelayOfChats()
        {
            var chats = Chats.Where(c => c.Statu == ChatStatu.Active);
            var chatIdsWillCloseDueToWaiting = new List<Guid>();
            foreach (var chat in chats)
            {
                chat.WaitingDuration++;
                if (chat.WaitingDuration >= WaitingDurationLimit)
                {
                    chat.Statu = ChatStatu.ClosedDueToWaiting;
                    chatIdsWillCloseDueToWaiting.Add(chat.Id);
                    //log chat

                }
                await ChatClosedDueToWaiting(chatIdsWillCloseDueToWaiting);


            }
        }
    }
}
