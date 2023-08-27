using Common.Enums;

namespace Entities
{
    public class Team : BaseEntity
    {
        public List<Agent> Agents { get; set; }
        public required string Name { get; set; }
        public TeamShiftType Type { get; set; }
        public TeamStatus Status { get; set; }
    }
}
