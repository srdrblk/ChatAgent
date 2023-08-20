using Common.Enums;

namespace Entities
{
    public class Message :BaseEntity
    {
        public required string Text { get; set; }
        public MessageDirection Direction { get; set; }
        public DateTime CreatedDate { get; set; }
  
    }
}
