using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MosadApiServer.Data;
using MosadApiServer.Enums;
using MosadApiServer.Interfaces;
using MosadApiServer.Models;
using MosadApiServer.Servises;
using MosadApiServer.Utils;
using Microsoft.AspNetCore.Authorization;


namespace MosadApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MissionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AgentsController> _logger;
        
        private ServiceMission _serviceMission;
        private readonly IServiceMoving _serviceMoving;
        private AgentsController _agentsController;

        public MissionsController(ILogger<AgentsController> logger, ApplicationDbContext context,
             ServiceMission serviceMission, IServiceMoving serviceMoving , AgentsController agentsController)
        {
            this._context = context;
            this._logger = logger;
            this._serviceMission = serviceMission;
            this._serviceMoving = serviceMoving;
            this._agentsController = agentsController;
        }

        [HttpGet()]
        public async Task<IActionResult> GetAllMission()
        {
            int status = StatusCodes.Status200OK;
            var missions = await this._context.Missions.ToListAsync();
            return Ok(missions);
        }
        [Authorize(Policy = "SimulationPolicy")]

        [HttpPost("Update")]
        public async Task<IActionResult> UpdatMissions()
        {
            int status;
            var missions = await this._context.Missions.Where(mission => mission.status == MissionStatuses.MITZVAHTASK).ToListAsync();
            if (missions.Count == 0)
            {
                status = StatusCodes.Status404NotFound;
                return StatusCode(status, HttpUtils.Response(status, "mission not found"));
            }

            foreach (var mission in missions)
            {
                var agent = await this._context.Agents.Include(a => a.Coordinate).FirstOrDefaultAsync(agent => agent.id == mission.agentId);
                var target = await this._context.Targets.Include(t => t.coordinate).FirstOrDefaultAsync(target => target.id == mission.targetId);
                var distance = await this._serviceMoving.GetDistance(agent.Coordinate, target.coordinate);
                if (agent.Coordinate.x == target.coordinate.x && agent.Coordinate.y == target.coordinate.y)
                {
                    target.status = TargetStatuses.ELIMINATED;
                    agent.status = AgentStatuses.DORMANT;
                    mission.status = MissionStatuses.ENDED;
                    status = StatusCodes.Status200OK;
                    missions.Remove(mission);
                    await this._context.SaveChangesAsync();

                    return StatusCode(status, HttpUtils.Response(status, new { target = target.status }));
                    

                }
                agent.Coordinate = await this._serviceMoving.Move(
                   await this._serviceMoving.GetDirectionAsync(agent.Coordinate, target.coordinate), target.coordinate);
                status = StatusCodes.Status200OK;
                await this._context.SaveChangesAsync();

                return StatusCode(status, HttpUtils.Response(status, new { agent = agent.Coordinate }));

            }
            status = StatusCodes.Status200OK;
            return StatusCode(status);
           
        }



        [Authorize(Policy = "MVCPolicy")]

        [HttpPut("{id}")]
        [Produces("application/json")]
        public async Task<IActionResult> PutStatusMission([FromBody] int id, MissionStatuses missionStatuses)
        {
            int status;
            var mission = await this._context.Missions.FirstOrDefaultAsync(mission => mission.id == id);
            if (mission == null)
            {
                status = StatusCodes.Status404NotFound;
                return StatusCode(status, HttpUtils.Response(status, "mission not found"));
            }

            var agent = await this._context.Agents.Include(a => a.Coordinate).FirstOrDefaultAsync(agent => agent.id == mission.agentId);
            var target = await this._context.Targets.Include(t => t.coordinate).FirstOrDefaultAsync(target => target.id == mission.targetId);
            var distance = await this._serviceMoving.GetDistance(agent.Coordinate, target.coordinate);
            if (distance > 200)
            {
                
                await this._serviceMission.OfferedMission();
            }
            else
            {
                mission.status = MissionStatuses.MITZVAHTASK;
                agent.status = AgentStatuses.INACTIVE;
                target.status = TargetStatuses.ISALIVE;
                mission.timeLeft = distance / 5;
                var missions = await this._context.Missions.ToListAsync();
                foreach (var missione in missions)
                {
                    if ((missione.agentId == agent.id || missione.targetId == target.id) &&
                        (mission.status != MissionStatuses.MITZVAHTASK))
                    {
                        missions.Remove(mission);
                    }
                }
            }
            status = StatusCodes.Status200OK;
            await this._context.SaveChangesAsync();
            return StatusCode(status, HttpUtils.Response(status, new { missions = mission }));


        }



        // double lastLocation = await ServiceMoving.GetDistance(agent.location, target.location);

        //mission.ActualExecutionTime += ServiceMoving.GetDistance(lastLocation, target.location);


        //return StatusCode(status, HttpUtils.Response(status, new { mission = mission }));

    }
    
}
        
               

        
        
           
        
    

            