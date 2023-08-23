using Business.IServices;
using Common.Enums;
using Common.Extensions;
using Core.Context;
using Dtos;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Business.Services
{
    public class TeamService : ITeamService
    {
        private AgentContext context { get; set; }
        private int MaximumConcurrency = 10;
        public TeamService(AgentContext _context)
        {
            context = _context;
        }

        public async Task<BaseResponse<bool>> ActivateTeam()
        {

            var currentHour = DateTime.Now.Hour;
            var teamType = TeamType.None;

            if (TeamType.DayShift.GetStartHour() <= currentHour && TeamType.DayShift.GetEndHour() >= currentHour)
            {
                teamType = TeamType.DayShift;
            }
            if (TeamType.EveningShift.GetStartHour() <= currentHour && TeamType.EveningShift.GetEndHour() >= currentHour)
            {
                teamType = TeamType.EveningShift;
            }
            if (TeamType.NightShift.GetStartHour() <= currentHour && TeamType.NightShift.GetEndHour() >= currentHour)
            {
                teamType = TeamType.NightShift;
            }
            if (teamType == TeamType.None)
            {
                return new BaseResponse<bool>() { Data = false, Message = "There is a problem with the shift hours!" };
            }
            var activeTeam = context.Teams.FirstOrDefault(t => t.Status == TeamStatus.Active);
            if (activeTeam != null)
            {
                // TODO : get active support chats
                var activeSupport = new List<Support>();
                if (activeSupport.Any())
                {
                    activeTeam.Status = TeamStatus.WaitingActiveChats;
                }
                else
                {
                    activeTeam.Status = TeamStatus.Passive;
                }

            }

            var team = context.Teams.FirstOrDefault(t => t.Type == teamType);
            if (team != null)
            {
                team.Status = TeamStatus.Active;
            }

            return await Task.Run(() => new BaseResponse<bool>()
            {
                Data = true,
                Message = teamType.GetDisplayName() + " is activated!"
            });
        }
        public async Task<BaseResponse<bool>> ActivateOverflowTeam()
        {
            var overflowTeam = context.Teams.FirstOrDefault(t => t.Type == TeamType.Overflow);

            if (overflowTeam == null)
            {
                return await Task.Run(() => new BaseResponse<bool>()
                {
                    Data = false,
                    Message = "Can not find " + TeamType.Overflow.GetDisplayName()
                });
            }

            overflowTeam.Status = TeamStatus.Active;

            return await Task.Run(() => new BaseResponse<bool>()
            {
                Data = true,
                Message = TeamType.Overflow.GetDisplayName() + " is activated!"
            });
        }
        private bool CheckAgentChatAvailability(Agent agent)
        {
            var effenciency = agent.Type.GetEffenciency();
            var maxChatCount = MaximumConcurrency * effenciency;

            return maxChatCount > agent.Chat.Count ? true : false;
        }
        public async Task<List<Agent>> CheckMainTeamAgents()
        {
            var agents = new List<Agent>();
            var team = await context.Teams.Include(t => t.Agents).ThenInclude(chat => chat.Chat).FirstOrDefaultAsync(t => t.Status == TeamStatus.Active && t.Type != TeamType.Overflow);
            foreach (var agent in team.Agents)
            {

                if (CheckAgentChatAvailability(agent))
                {
                    agents.Add(agent);
                }
            }
            return agents;
        }
        public async Task<List<Agent>> CheckOverflowTeamAgents()
        {
            var agents = new List<Agent>();
            var team = await context.Teams.Include(t => t.Agents).ThenInclude(chat => chat.Chat).FirstOrDefaultAsync(t => t.Status == TeamStatus.Active && t.Type == TeamType.Overflow);
            foreach (var agent in team.Agents)
            {

                if (CheckAgentChatAvailability(agent))
                {
                    agents.Add(agent);
                }
            }
            return agents;
        }
        public async Task<Agent?> GetAvailableAgent()
        {
            var availableAgents = new List<Agent>();
            var mainTeamAgents = await CheckMainTeamAgents();

            if (!mainTeamAgents.Any())
            {
                var overflowAgents = await CheckOverflowTeamAgents();
                availableAgents.AddRange(overflowAgents);
            }
            else
            {
                availableAgents.AddRange(mainTeamAgents);
            }
          


            return availableAgents?.OrderBy(mta => mta.Type)?.OrderBy(mta=>mta.Chat?.Count)?.FirstOrDefault();
        }
    }

}
