using Business.IServices;
using Common.Enums;
using Common.Extensions;
using Core.Context;
using Dtos;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;

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
        public async Task<TeamShiftType> GetTeamTypeForCurrentShift()
        {
            var currentHour = DateTime.Now.Hour;
            var teamType = TeamShiftType.None;
            if (TeamShiftType.DayShift.GetStartHour() <= currentHour && TeamShiftType.DayShift.GetEndHour() >= currentHour)
            {
                teamType = TeamShiftType.DayShift;
            }
            else if (TeamShiftType.EveningShift.GetStartHour() <= currentHour && TeamShiftType.EveningShift.GetEndHour() >= currentHour)
            {
                teamType = TeamShiftType.EveningShift;
            }
            else if (TeamShiftType.NightShift.GetStartHour() <= currentHour && TeamShiftType.NightShift.GetEndHour() >= currentHour)
            {
                teamType = TeamShiftType.NightShift;
            }
            else
            {
                teamType = TeamShiftType.None;
            }
            return await Task.Run(() => teamType);
        }
        public async Task<BaseResponse<Team>> ActivateTeam()
        {

            var currentHour = DateTime.Now.Hour;
            var teamType = await GetTeamTypeForCurrentShift();

            if (teamType == TeamShiftType.None)
            {
                return new BaseResponse<Team>() { Statu = ResponseStatu.Error, Message = "There is a problem with the shift hours!" };
            }
            var activeTeam = await context.Teams.Include(t => t.Agents).ThenInclude(a => a.Chat).SingleOrDefaultAsync(t => t.Status == TeamStatus.Active && t.Type != TeamShiftType.Overflow);
            if (activeTeam != null)
            {
                var haveActiveChat = activeTeam.Agents.Any() && activeTeam.Agents.Any(a => a.Chat.Any(c => c.Statu == ChatStatu.Active));
                activeTeam.Status = haveActiveChat ? TeamStatus.WaitingActiveChats : TeamStatus.Passive;
            }

            var team = await context.Teams.SingleOrDefaultAsync(t => t.Type == teamType);
            if (team != null)
            {
                team.Status = TeamStatus.Active;
                await context.SaveChangesAsync();

                return await Task.Run(() => new BaseResponse<Team>()
                {
                    Data = team,
                    Statu = ResponseStatu.Success,
                    Message = teamType.GetDisplayName() + " is activated!"
                });
            }
            return new BaseResponse<Team>() { Statu = ResponseStatu.Error, Message = "Can't find Team by current shift ()!" };


        }
        public async Task<BaseResponse<bool>> ActivateOverflowTeam()
        {
            var overflowTeam = await context.Teams.SingleOrDefaultAsync(t => t.Type == TeamShiftType.Overflow);

            if (overflowTeam == null)
            {
                return await Task.Run(() => new BaseResponse<bool>()
                {
                    Data = false,
                    Message = "Can not find " + TeamShiftType.Overflow.GetDisplayName()
                });
            }

            overflowTeam.Status = TeamStatus.Active;
            try
            {
                context.Entry(overflowTeam).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return await Task.Run(() => new BaseResponse<bool>()
            {
                Data = true,
                Message = TeamShiftType.Overflow.GetDisplayName() + " is activated!"
            });
        }
        public async Task<BaseResponse<bool>> DeActivateOverflowTeam()
        {
            var overflowTeam = await context.Teams.SingleOrDefaultAsync(t => t.Type == TeamShiftType.Overflow);

            if (overflowTeam == null)
            {
                return await Task.Run(() => new BaseResponse<bool>()
                {
                    Data = false,
                    Message = "Can not find " + TeamShiftType.Overflow.GetDisplayName()
                });
            }

            overflowTeam.Status = TeamStatus.Passive;
            try
            {
                context.Entry(overflowTeam).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return await Task.Run(() => new BaseResponse<bool>()
            {
                Data = true,
                Message = TeamShiftType.Overflow.GetDisplayName() + " is activated!"
            });
        }
        public async Task<bool> GetAgentChatAvailability(Agent agent)
        {
            var maxChatCount = MaximumConcurrency * agent.Type.GetEffenciency();

            return await Task.Run(() => maxChatCount > agent.Chat.Count ? true : false);
        }
        public async Task<List<Agent>> GetActiveTeamAgents()
        {
            var agents = new List<Agent>();
            var team = await context.Teams.Select(t =>
                new Team()
                {
                    Name = t.Name,
                    Agents = t.Agents.Select(a =>
                    new Agent()
                    {
                        Chat = a.Chat.Where(c => c.Statu == ChatStatu.Active).ToList(),
                        Id = a.Id,
                        Name = a.Name,
                        Status = a.Status,
                        TeamId = a.TeamId,
                        Type = a.Type
                    }
                     ).ToList(),
                    Id = t.Id,
                    Status = t.Status,
                    Type = t.Type,
                }
                ).FirstOrDefaultAsync(t => t.Status == TeamStatus.Active && t.Type != TeamShiftType.Overflow);
            if (team != null)
            {
                foreach (var agent in team.Agents)
                {
                    if (await GetAgentChatAvailability(agent))
                    {
                        agents.Add(agent);
                    }
                }
            }
            return agents;
        }

        public async Task<List<Agent>> GetOverflowTeamAgents()
        {
            var agents = new List<Agent>();
            var team = await context.Teams.Select(t =>
                new Team()
                {
                    Id = t.Id,
                    Status = t.Status,
                    Type = t.Type,
                    Name = t.Name,
                    Agents = t.Agents.Select(a =>
                    new Agent()
                    {
                        Chat = a.Chat.Where(c => c.Statu == ChatStatu.Active).ToList(),
                        Id = a.Id,
                        Name = a.Name,
                        Status = a.Status,
                        TeamId = a.TeamId,
                        Type = a.Type
                    }
                     ).ToList(),

                }
                ).FirstOrDefaultAsync(t => t.Status == TeamStatus.Active && t.Type == TeamShiftType.Overflow);
            if (team != null)
            {
                foreach (var agent in team.Agents)
                {
                    if (await GetAgentChatAvailability(agent))
                    {
                        agents.Add(agent);
                    }
                }
            }
            return agents;
        }
        public async Task<Agent?> GetAvailableAgent()
        {
            var availableAgents = new List<Agent>();
            var mainTeamAvailableAgents = await GetActiveTeamAgents();

            if (!mainTeamAvailableAgents.Any())
            {
                var overflowAgents = await GetOverflowTeamAgents();
                availableAgents.AddRange(overflowAgents);
            }
            else
            {
                availableAgents.AddRange(mainTeamAvailableAgents);
            }

            return await Task.Run(() => availableAgents?.OrderBy(mta => mta.Type)?.OrderBy(mta => mta.Chat?.Count)?.FirstOrDefault());
        }

        public async Task<List<Team>> GetTeams()
        {
            return await context.Teams.ToListAsync();
        }
        public async Task<bool> CheckTeamIsActiveByTeamShiftType(TeamShiftType teamShiftType)
        {
            var isActive = await context.Teams.AnyAsync(t => t.Status == TeamStatus.Active && t.Type == teamShiftType);
            return isActive;
        }
        public async Task<bool> CloseTeamThatNotActiveIfDoNotHaveActiveChats()
        {
            var teamThatWaitingActiveChats = await context.Teams.SingleOrDefaultAsync(t => t.Status == TeamStatus.WaitingActiveChats);
            if (teamThatWaitingActiveChats == null)
            {
                return false;
            }

            return !await CheckTeamHaveAnyActiveChatsByTeamId(teamThatWaitingActiveChats.Id);
        }
        private async Task<bool> CheckTeamHaveAnyActiveChatsByTeamId(long teamId)
        {
            return await context.Chats.AnyAsync(c => c.Agent.TeamId == teamId); ;
        }

        public async Task<bool> PassiveTheStatusOfTheTeamWaitingForActiveChats()
        {
            try
            {
                var teamThatWaitingActiveChats = await context.Teams.SingleOrDefaultAsync(t => t.Status == TeamStatus.WaitingActiveChats);
                if (teamThatWaitingActiveChats != null)
                {
                    teamThatWaitingActiveChats.Status = TeamStatus.Passive;
                    await context.SaveChangesAsync();

                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return false;

        }
    }

}
