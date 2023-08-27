using Common.Enums;
using Core.Seeds;
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

        public AgentContext()
        {

        }
        public AgentContext(DbContextOptions<AgentContext> options) : base(options)
        {
           // ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Team>().HasData(new TeamSeed().GetTeams());
            modelBuilder.Entity<Agent>().HasData(new AgentSeed().GetAgents());

            base.OnModelCreating(modelBuilder);
        }
    }
}
