using Business.IServices;
using Business.Queues;
using Common.Enums;
using Core.Context;
using Dtos;
using Entities;

namespace Business.Services
{
    public class ChatService : IChatService
    {

        private AgentContext context { get; set; }
        private int WaitingDurationLimit = 3;
        ChatQueue chatQueue;
        private ITeamService teamService;
        private IChatHubService chatHubService;
        public ChatService(ChatQueue _chatQueue, AgentContext _context, ITeamService _teamService, IChatHubService _chatHubService)
        {
            chatQueue = _chatQueue;
            context = _context;
            teamService = _teamService;
            chatHubService = _chatHubService;
        }
        public async Task<BaseResponse<string>> CreateChat(SupportDto supportDto, long userId)
        {
            try
            {
                var support = new Chat()
                {
                    Subject = supportDto.Subject,
                    CreatedDate = DateTime.Now,
                    User = new User()
                    {
                        Id = userId,
                        FullName = supportDto.User.FullName,
                    },


                };
                chatQueue.AddChatToQueue(support);
            }
            catch (Exception)
            {

                throw;
            }
            return await Task.Run(() => new BaseResponse<string> { Statu = Common.Enums.ResponseStatu.Success });
        }
        public async Task AddChat(Chat chat)
        {

            context.Chats.Add(chat);
            await context.SaveChangesAsync();
        }
        public async Task<BaseResponse<Message>> AddMessageToChat(long chatId, Message message)
        {
            var response = new BaseResponse<Message>();
            var chat = context.Chats.FirstOrDefault(c => c.Id == message.ChatId);
            if (chat == null || chat.Statu != ChatStatu.Active)
            {
                response.Statu = ResponseStatu.Error;
                response.Message = "Chat is not exist!";
                return response;
            }
            context.Messages.Add(message);
            await context.SaveChangesAsync();
            response.Statu = ResponseStatu.Success;

            return response;

        }
        public async Task ChatCompleted(long chatId)
        {

            var chat = context.Chats.FirstOrDefault(c => c.Id == chatId);
            if (chat != null)
            {
                chat.Statu = ChatStatu.SupportCompleted;
                await context.SaveChangesAsync();
            }
        }

        public async Task CheckDelayOfChats()
        {
            var chats = context.Chats.Where(c => c.Statu == ChatStatu.Active);
            var chatIdsWillCloseDueToWaiting = new List<long>();
            foreach (var chat in chats)
            {
                chat.WaitingDuration++;
                if (chat.WaitingDuration >= WaitingDurationLimit)
                {
                    chat.Statu = ChatStatu.ClosedDueToWaiting;
                    chatIdsWillCloseDueToWaiting.Add(chat.Id);
                    //log chat
                }
            }
            await context.SaveChangesAsync();

        }

    }
}
