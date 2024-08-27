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
        
        public DbSet<Coordinates> Coordinates { get; set; }

       
    }
}
