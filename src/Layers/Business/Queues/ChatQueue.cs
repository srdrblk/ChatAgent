using Dtos;
using Entities;

namespace Business.Queues
{
    public class ChatQueue
    {
        private Queue<Chat> Queue { get; set; } = new Queue<Chat>();
        public ChatQueue() { }

        public void AddChatToQueue(Chat chat)
        {
            Queue.Enqueue(chat);
        }
        public async Task<Chat?> GetAndRemoveChatFromQueue()
        {
            if (Queue.Count == 0)
                return null;
            return await Task.Run(() => Queue.Dequeue());
        }
    }
}
