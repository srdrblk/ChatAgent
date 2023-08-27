using Common.Enums;
using Entities;

namespace Core.Seeds
{
    public class TeamSeed
    {
        public TeamSeed() { }
        public List<Team> GetTeams()
        {
            return new List<Team>()
            {
               new Team { Name = "A", Type = TeamShiftType.DayShift, Id = 1, Status = TeamStatus.Active },
                new Team() { Name = "B", Type = TeamShiftType.EveningShift, Id = 2 },
                new Team() { Name = "C", Type = TeamShiftType.NightShift, Id = 3 },
                new Team() { Name = "O", Type = TeamShiftType.Overflow, Id = 4 }
            };
        }
    }
}
