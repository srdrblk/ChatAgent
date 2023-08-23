using Common.Enums;

namespace Entities
{
    public class Chat :BaseEntity
    {
        public string Subject { get; set; }
        public required User User { get; set; }
        public  Agent Agent { get; set; }
        public int WaitingDuration { get; set; }
        public ChatStatu Statu { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<Message> Messages { get; set; } = new List<Message>();
        public Chat()
        {
        }
    }
}
