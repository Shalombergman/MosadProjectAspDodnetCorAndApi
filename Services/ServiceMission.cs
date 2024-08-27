using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.SqlServer.Server;
using MosadApiServer.Data;
using MosadApiServer.Enums;
using MosadApiServer.Interfaces;
using MosadApiServer.Models;
using MosadApiServer.Utils;


namespace MosadApiServer.Servises
{
    public class ServiceMission
    {
        private  ApplicationDbContext _context;
        //private readonly IServiceMoving _serviceMoving;

        public ServiceMission(ApplicationDbContext context  /*IServiceMoving serviceMoving*/)
        {
            this._context = context;
            //this._serviceMoving = serviceMoving;

        }

        public async Task OfferedMission()
        {

            // var agentsOffered = new List<Agent>();
            // var targetsOffered = new List<Target>();
            var agents = await this._context.Agents.Include(a => a.Coordinate).Where(a => a.status == AgentStatuses.DORMANT).ToListAsync();
            var targets = await this._context.Targets.Include(t => t.coordinate).Where(t => t.status == TargetStatuses.ISALIVE).ToListAsync();

            if (agents == null || targets == null)
            {
                throw new Exception("the agent or targets not avalibal");
            }
            else
            {
                foreach (var agent in agents)
                {
                    if (agent.Coordinate != null)
                    {
                        foreach (var target in targets)
                        {
                            if (target.coordinate != null)
                            {
                                var distance = await GetDistance(agent.Coordinate, target.coordinate);

                                if (distance <= 200)
                                {
                                    Mission mission = new Mission()
                                    {
                                        agentId = agent.id,
                                        targetId = target.id,
                                        status = MissionStatuses.PROPOSAL,
                                        timeLeft = distance / 5,

                                    };
                                    await this._context.Missions.AddAsync( mission);
                                    await this._context.SaveChangesAsync();

                                }
                            }
                           

                        }
                    }
                    
                }

            }
            
        }
        public async Task<double> GetDistance(Coordinates coordinates1, Coordinates coordinates2)
        {
           
            if((coordinates1.x == null || coordinates1.y == null)||(coordinates2.x == null || coordinates2.y == null))
            {
                 throw new ArgumentNullException ("Agent location is not set.");
            }
            int equalX = coordinates2.x - coordinates1.x;
            int equalY = coordinates2.y - coordinates1.y;
            int absoluteValueX = Math.Abs(equalX);
            int absoluteValueY = Math.Abs(equalY);
            return Math.Sqrt(Math.Pow(absoluteValueX, 2) + Math.Pow(absoluteValueY, 2));
            await this._context.SaveChangesAsync();

        }

    }
}