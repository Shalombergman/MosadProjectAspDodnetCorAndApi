
using System.Diagnostics.Metrics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MosadApiServer.Data;
using MosadApiServer.Enums;
using MosadApiServer.Interfaces;
using MosadApiServer.Models;
using MosadApiServer.Servises;
using MosadApiServer.Utils;


namespace MosadApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly Coordinates _coordinates = new Coordinates();
     
        private readonly IServiceMoving _serviceMoving;
        private readonly ServiceMission _serviceMission;

        public AgentsController(ApplicationDbContext context, ServiceMission serviceMission, IServiceMoving serviceMoving)
        {
            this._context = context;
            this._serviceMission = serviceMission;
            this._serviceMoving = serviceMoving;
        }



        [HttpGet]
        public async Task<IActionResult> GetAllAgent()
        {
            int status = StatusCodes.Status200OK;
            var agents = await this._context.Agents.Include(a=>a.Coordinate).ToListAsync();
            return Ok(agents);
        }


        [Authorize(Policy = "SimulationPolicy")]
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateAttack(Agent agent)
        {   
            this._context.Agents.Add(agent);
            await this._context.SaveChangesAsync();
            return StatusCode(
                StatusCodes.Status201Created,
                new { success = true, agent = agent.id }

                );
            await this._context.SaveChangesAsync();
        }
        [Authorize(Policy = "SimulationPolicy")]
        [HttpPut("{id}/pin")]
        public async Task<IActionResult> AgentPin(int id,[FromBody] Coordinates coordinates)
        {
            int status;
            Agent agent = await this._context.Agents.FirstOrDefaultAsync(agents => agents.id == id);
            if (agent == null)
            {
                status = StatusCodes.Status404NotFound;
                return StatusCode(status, HttpUtils.Response(status, "agent not found"));
            }
            agent.Coordinate = await this._serviceMoving.CreatPinlocation(coordinates.x, coordinates.y);
            status = StatusCodes.Status200OK;
            await this._context.SaveChangesAsync();
            await this._serviceMission.OfferedMission();
            return StatusCode(status, HttpUtils.Response(status, new { agent = agent }));
        }



        [Authorize(Policy = "SimulationPolicy")]
        [HttpPut("{id}/move")]
        public async Task<IActionResult> AgentMove(int id ,Direction direction )
        {
            Direction direction1 = direction;
            int status;
            Agent agent = await this._context.Agents.Include(a => a.Coordinate).FirstOrDefaultAsync(agents => agents.id == id);
            if (agent == null)
            {
                status = StatusCodes.Status404NotFound;
                return StatusCode(status, HttpUtils.Response(status, "agent not found"));
            }
            var coordinates = agent.Coordinate; 
            if (coordinates == null)
            {
                return BadRequest("Agent location is not set.");
            }
            agent.Coordinate = await this._serviceMoving.Move(direction1 , coordinates, 1,1);
            status = StatusCodes.Status200OK;
            await this._context.SaveChangesAsync();
            await this._serviceMission.OfferedMission();
            return StatusCode(status, HttpUtils.Response(status, new { agent = agent.Coordinate }));
        }



        

    }
}
