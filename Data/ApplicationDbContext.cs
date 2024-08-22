using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using MosadApiServer.Models;

namespace MosadApiServer.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<Agent> Agents { get; set; }
        public DbSet<Mission> Missions { get; set; }
        public DbSet<Target> Targets { get; set; }
        public DbSet<Coordinates> Locations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Agent>()
                .HasMany(e => e.targets)
                .WithMany(e => e.agents)
                .UsingEntity<Mission>(
                    l => l.HasOne<Target>(e => e.target).WithMany(e => e.missions),
                    r => r.HasOne<Agent>(e => e.agent).WithMany(e => e.missions));
        }
    }
}
