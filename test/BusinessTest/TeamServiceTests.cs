using Business.IServices;
using Business.Services;
using Common.Enums;
using Common.Extensions;
using Core.Context;
using Core.Seeds;
using Entities;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Diagnostics.Metrics;

namespace BusinessTest;

public class TeamServiceTests : IDisposable
{
    ITeamService teamService;
    AgentContext agentContext;
    Chat chat = new Chat() { User = new User() { FullName = "test" }, Statu = ChatStatu.Active };
    [OneTimeSetUp]
    public void Setup()
    {
        var options = new  DbContextOptionsBuilder<AgentContext>()
         .UseInMemoryDatabase(databaseName: "TeamTest_AgentContextDatabase")
         .Options;

        agentContext = new AgentContext(options);
   
        agentContext.Teams.AddRange(new TeamSeed().GetTeams());
        agentContext.Agents.AddRange(new AgentSeed().GetAgents());
        agentContext.SaveChangesAsync().Wait();
        teamService = new TeamService(agentContext);
    }


    [Test, Order(1)]
    public async Task ActivateOverflowTeam_ShouldReturnTwo_WhenCallIt()
    {
        await teamService.ActivateOverflowTeam();
        var teams = await teamService.GetTeams();
        var activeTeamCount = teams.Where(t => t.Status == TeamStatus.Active).Count();
        Assert.That(activeTeamCount, Is.EqualTo(2));
    }
    [Test, Order(2)]
    public async Task DeActivateOverflowTeam_ShouldReturnPasive_WhenCallIt()
    {
        await teamService.DeActivateOverflowTeam();
        var teams = await teamService.GetTeams();
        var overflowTeam = teams.SingleOrDefault(t => t.Type == TeamShiftType.Overflow);
        Assert.That(TeamStatus.Passive, Is.EqualTo(overflowTeam.Status));
    }

    [Test, Order(3)]
    public async Task ActivateTeam_ShouldReturnOne_WhenCallIt()
    {
        var response = await teamService.ActivateTeam();
        var teams = await teamService.GetTeams();
        var activeTeamCount = teams.Where(t => t.Status == TeamStatus.Active).Count();
        Assert.That(activeTeamCount, Is.EqualTo(1));
    }

    [Test, Order(4)]
    public async Task GetTeamTypeForCurrentShift_ShouldReturnTrue_WhenCheckWithCurrentHour()
    {
        var currentHour = DateTime.Now.Hour;
        var activeTeamType = await teamService.GetTeamTypeForCurrentShift();

        Assert.That(currentHour, Is.AtLeast(activeTeamType.GetStartHour()));
        Assert.That(currentHour, Is.AtMost(activeTeamType.GetEndHour()));
    }
    [Test, Order(5)]
    public async Task GetAgentChatAvailability_ShouldReturnTrue_WhenHaveZeroChat()
    {
        var agent = new Agent() { Chat = new List<Chat>(), Type = AgentType.Junior };
        var isAvailable = await teamService.GetAgentChatAvailability(agent);
        Assert.That(isAvailable, Is.EqualTo(true));
    }

    [Test, Order(6)]
    public async Task GetActiveTeamAgents_ShouldReturnTrue_WhenCheckAgentIds()
    {
        var activeTeamType = await teamService.GetTeamTypeForCurrentShift();
        var activeTeam = new TeamSeed().GetTeams().SingleOrDefault(t => t.Type == activeTeamType);
        var activeTeamAgents = new AgentSeed().GetAgents().Where(t => t.TeamId == activeTeam.Id);
        var responseActivateTeam = await teamService.ActivateTeam();
        Assert.That(responseActivateTeam.Statu, Is.EqualTo(ResponseStatu.Success));
        var _activeTeamAgents = await teamService.GetActiveTeamAgents();

        Assert.That(activeTeamAgents.Select(a => a.Id), Is.EqualTo(_activeTeamAgents.Select(a => a.Id)));
    }
    [Test, Order(7)]
    public async Task GetOverflowTeamAgents_ShouldReturnTrue_WhenCheckAgentIds()
    {
        var overflowTeam = new TeamSeed().GetTeams().SingleOrDefault(t => t.Type == TeamShiftType.Overflow);
        var activeTeamAgents = new AgentSeed().GetAgents().Where(t => t.TeamId == overflowTeam.Id);
        await teamService.ActivateOverflowTeam();
        var _activeTeamAgents = await teamService.GetOverflowTeamAgents();

        Assert.That(activeTeamAgents.Select(a => a.Id), Is.EqualTo(_activeTeamAgents.Select(a => a.Id)));
    }

    [Test, Order(8)]
    public async Task GetAvailableAgent_ShouldReturnTrue_WhenCheckAvailableAgentId()
    {
        var activeTeamType = await teamService.GetTeamTypeForCurrentShift();
        var activeTeam = new TeamSeed().GetTeams().SingleOrDefault(t => t.Type == activeTeamType);
        var availableTeamAgents = new AgentSeed().GetAgents().Where(t => t.TeamId == activeTeam.Id);
        var availableTeamAgent = availableTeamAgents.OrderBy(mta => mta.Type)?.OrderBy(mta => mta.Chat?.Count)?.FirstOrDefault();
        var responseActivateTeam = await teamService.ActivateTeam();
        Assert.That(responseActivateTeam.Statu, Is.EqualTo(ResponseStatu.Success));
        var _activeTeamAgents = await teamService.GetAvailableAgent();
        Assert.That(availableTeamAgent.Id, Is.EqualTo(_activeTeamAgents.Id));
    }
    public void Dispose()
    {
       agentContext.Dispose();
       
    }

}