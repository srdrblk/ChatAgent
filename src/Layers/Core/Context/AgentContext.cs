using Common.Enums;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Context
{
    public class AgentContext : DbContext
    {
  
        public DbSet<Agent> Agents { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Support> Supports { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<User> Users { get; set; }

        public AgentContext(DbContextOptions<AgentContext> options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.LazyLoadingEnabled = false;
        }
  
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Team>().HasData(
                new Team { Name = "A", Type = TeamType.DayShift, Id = 1 , Status = TeamStatus.Active},
                new Team() { Name = "B", Type = TeamType.EveningShift, Id = 2 },
                new Team() { Name = "C", Type = TeamType.NightShift, Id = 3 },
                new Team() { Name = "O", Type = TeamType.Overflow, Id = 4 }
                );
            modelBuilder.Entity<Agent>().HasData(
                new Agent() { Id = 1, Name = "A1", Status = AgentStatus.Active, Type = AgentType.TeamLead, TeamId = 1 },
                new Agent() { Id = 2, Name = "A2", Status = AgentStatus.Active, Type = AgentType.MidLevel, TeamId = 1 },
                new Agent() { Id = 3, Name = "A3", Status = AgentStatus.Active, Type = AgentType.MidLevel, TeamId = 1 },
                new Agent() { Id = 4, Name = "A4", Status = AgentStatus.Active, Type = AgentType.Junior, TeamId = 1 },
                new Agent() { Id = 5, Name = "B1", Status = AgentStatus.Active, Type = AgentType.Senior, TeamId = 2 },
                new Agent() { Id = 6, Name = "B2", Status = AgentStatus.Active, Type = AgentType.MidLevel, TeamId = 2 },
                new Agent() { Id = 7, Name = "B3", Status = AgentStatus.Active, Type = AgentType.Junior, TeamId = 2 },
                new Agent() { Id = 8, Name = "B4", Status = AgentStatus.Active, Type = AgentType.Junior, TeamId = 2 },
                new Agent() { Id = 9, Name = "C1", Status = AgentStatus.Active, Type = AgentType.MidLevel, TeamId = 3 },
                new Agent() { Id = 10, Name = "C2", Status = AgentStatus.Active, Type = AgentType.MidLevel, TeamId = 3 },
                new Agent() { Id = 11, Name = "O1", Status = AgentStatus.Active, Type = AgentType.Junior,  TeamId = 4 },
                new Agent() { Id = 12, Name = "O2", Status = AgentStatus.Active, Type = AgentType.Junior, TeamId = 4 },
                new Agent() { Id = 13, Name = "O3", Status = AgentStatus.Active, Type = AgentType.Junior, TeamId = 4 },
                new Agent() { Id = 14, Name = "O4", Status = AgentStatus.Active, Type = AgentType.Junior, TeamId = 4 },
                new Agent() { Id = 15, Name = "O5", Status = AgentStatus.Active, Type = AgentType.Junior, TeamId = 4 },
                new Agent() { Id = 16, Name = "O6", Status = AgentStatus.Active, Type = AgentType.Junior, TeamId = 4 }
                );
            base.OnModelCreating(modelBuilder);
        }
    }
}
