using Common.Attributes;

namespace Common.Enums
{
    public enum AgentType
    {
        [Efficiency(Value: 0.4)]
        Junior = 0,
        [Efficiency(Value: 0.6)]
        MidLevel = 1,
        [Efficiency(Value: 0.8)]
        Senior = 2,
        [Efficiency(Value: 0.5)]
        TeamLead = 3
    }
}
