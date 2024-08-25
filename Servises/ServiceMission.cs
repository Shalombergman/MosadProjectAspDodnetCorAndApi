using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.SqlServer.Server;
using MosadApiServer.Data;
using MosadApiServer.Models;


namespace MosadApiServer.Servises
{
    public class ServiceMission
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ServiceMission> _logger;
        public Mission mission = new Mission();

        public ServiceMission(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<(List<Agent> agentsOffered, List<Target> targetsOffered)> OfferedMission()
        {

            var agentsOffered = new List<Agent>();
            var targetsOffered = new List<Target>();
            var agents = await _context.Agents.Include(a => a.location).Include(a => a.status).ToListAsync();
            var target = await _context.Targets.Include(t => t.location).Include(t => t.status).FirstOrDefaultAsync(t => t.id == mission.targetId);

            if (target == null || target.status != Enums.TargetStatuses.ISALIVE)
            {
                return new BadRequest("Target not found or not alive.");
            }
            foreach (var agent in agents)
            {
                if (agent.status == Enums.AgentStatuses.DORMANT)
                {
                    var distance = await ServiceMoving.GetDistance(agent.location, target.location);
                    if (distance <= 200)
                    {
                        targetsOffered.Add(target);
                        agentsOffered.Add(agent);
                    }
                }
            }
            return (agentsOffered, targetsOffered);


        }





    }
}
