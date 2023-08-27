using Common.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class Message :BaseEntity
    {
        public required string Text { get; set; }
        public MessageDirection Direction { get; set; }
        public DateTime CreatedDate { get; set; }
        public long ChatId { get; set; }

        [ForeignKey("ChatId")]
        public  Chat Chat { get; set; }
    }
}
