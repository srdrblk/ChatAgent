using Common.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class Agent : BaseEntity
    {
        public string Name { get; set; }
        public AgentStatus Status { get; set; }
        public AgentType Type { get; set; }
        public List<Chat> Chat { get; set; }

        [ForeignKey("TeamId")]
        public Team Team { get; set; }

        public long TeamId { get; set; }
    }
}
