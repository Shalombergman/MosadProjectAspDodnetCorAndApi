
using System.Diagnostics.Metrics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MosadApiServer.Data;
using MosadApiServer.Enums;
using MosadApiServer.Models;
using MosadApiServer.Utils;


namespace MosadApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AgentsController> _logger;
        private readonly Coordinates _coordinates;
        public AgentsController(ILogger<AgentsController> logger, ApplicationDbContext context, Coordinates coordinates)
        {
            this._context = context;
            this._logger = logger;
            this._coordinates = coordinates;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAgent()
        {
            int status = StatusCodes.Status200OK;
            var agents = await this._context.Agents.Include(a => a.location).ToListAsync();
            return Ok(agents);
        }

        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateAttack(Agent agent)
        {   
            this._context.Agents.Add(agent);
            await this._context.SaveChangesAsync();
            return StatusCode(
                StatusCodes.Status201Created,
                new { success = true, agent = agent }
                );
        }
        [HttpPut("{id}/move")]
        public async Task<IActionResult> AgentMove(int id ,Direction direction )
        {
            Direction direction1 = direction;
            int status;
            Agent agent = await this._context.Agents.FirstOrDefaultAsync(agents => agents.id == id);
            if (agent == null)
            {
                status = StatusCodes.Status404NotFound;
                return StatusCode(status, HttpUtils.Response(status, "agent not found"));
            }
            agent.location = await Data.LogicToMoving.Move(direction1 ,agent.location ,1,1);
            status = StatusCodes.Status200OK;
            return StatusCode(status, HttpUtils.Response(status, new { agent = agent }));
        }

        [HttpPut("{id}/pin")]
        public async Task<IActionResult> AgentPin(int id ,Coordinates location)
        {
            int status;
            Agent agent = await this._context.Agents.FirstOrDefaultAsync(agents => agents.id == id);
            if (agent == null)
            {
                status = StatusCodes.Status404NotFound;
                return StatusCode(status, HttpUtils.Response(status, "agent not found"));
            }
            agent.location = await Data.LogicToMoving.CreatPinlocation(location.x, location.y);
            status = StatusCodes.Status200OK;
            return StatusCode(status, HttpUtils.Response(status, new { agent = agent }));
        }

        

    }
}
