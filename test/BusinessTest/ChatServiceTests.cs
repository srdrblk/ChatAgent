using Business.IServices;
using Business.Queues;
using Business.Services;
using Common.Enums;
using Core.Context;
using Core.Seeds;
using Dtos;
using Entities;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace BusinessTest
{
    public class ChatServiceTests : IDisposable
    {
        AgentContext agentContext;
        ITeamService teamService;
        IChatService chatService;
        ChatQueue chatQueue { get; set; }
        IChatHubService chatHubService { get; set; }
        Chat chat = new Chat() { User = new User() { FullName = "Test User" }, Subject = "Test Chat" };
        [OneTimeSetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AgentContext>()
             .UseInMemoryDatabase(databaseName: "ChatTest_AgentContextDatabase")
             .Options;

            agentContext = new AgentContext(options);
            agentContext.Teams.AddRange(new TeamSeed().GetTeams());
            agentContext.Agents.AddRange(new AgentSeed().GetAgents());
            agentContext.SaveChangesAsync().Wait();
            chatQueue = new ChatQueue();
            chatHubService = new Mock<IChatHubService>().Object;
            teamService = new TeamService(agentContext);
            chatService = new ChatService(chatQueue, agentContext, teamService, chatHubService);
        }

        [Test, Order(1)]
        public async Task CreateChat_ShouldReturnTrue_WhenAddChatAndCheckAvailableAgentId()
        {

            var support = new SupportDto() { Subject = "Test Subject", User = new UserDto() { Email = "test@email.com", FullName = "Test User" } };
            var response = await chatService.AddChatToQueue(support, 1);
            Assert.That(response.Statu, Is.EqualTo(ResponseStatu.Success));
            var chatFromQueue = await chatService.GetAndRemoveChatFromQueue();
            Assert.That(chatFromQueue.Subject, Is.EqualTo(support.Subject));
        }
        [Test, Order(2)]
        public async Task GetAvailableAgent_ShouldReturnTrue_WhenAddChatAndCheckAvailableAgentId()
        {
            var activeTeamType = await teamService.GetTeamTypeForCurrentShift();
            var activeTeam = new TeamSeed().GetTeams().SingleOrDefault(t => t.Type == activeTeamType);
            var availableTeamAgents = new AgentSeed().GetAgents().Where(t => t.TeamId == activeTeam.Id);
            var availableTeamAgent = availableTeamAgents.OrderBy(mta => mta.Type)?.OrderBy(mta => mta.Chat?.Count)?.FirstOrDefault();
            chat.AgentId = availableTeamAgent.Id;
            chat.Statu = ChatStatu.Active;
            await chatService.SetChatToAgent(chat);
            availableTeamAgent.Chat = new List<Chat> { chat };
            /* Get new available agent after add chat to available agent*/
            availableTeamAgent = availableTeamAgents.OrderBy(mta => mta.Type)?.OrderBy(mta => mta.Chat?.Count)?.FirstOrDefault();
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
}
