using Common.Enums;

namespace Entities
{
    public class Chat :BaseEntity
    {
        public required User User { get; set; }
        public required Agent Agent { get; set; }
        public int WaitingDuration { get; set; }
        public ChatStatu Statu { get; set; }
        public Queue<Message> Messages { get; set; }
    }
}
