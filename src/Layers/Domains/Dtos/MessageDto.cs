using Common.Enums;

namespace Dtos
{
    public class MessageDto
    {
        public required string Text { get; set; }
        public MessageDirection Direction { get; set; }
    }
}
