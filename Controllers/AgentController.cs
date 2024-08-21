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
    public class AgentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AgentController> _logger;
        public AgentController(ILogger<AgentController> logger, ApplicationDbContext context)
        {
            this._context = context;
            this._logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAgent()
        {
            int status = StatusCodes.Status200OK;
            var agents = await this._context.Agents.ToListAsync();
            return Ok(agents);
        }

        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateAttack(Agent agent)
        {
            //attack.id = Guid.NewGuid();
            
            this._context.Agents.Add(agent);
            await this._context.SaveChangesAsync();
            return StatusCode(
                StatusCodes.Status201Created,
                new { success = true, agent = agent }
                );
        }

        [HttpGet("{id}/move")]
        public async Task<IActionResult> AgentStatus(int id)
        {
            int status;
            Agent agent = await this._context.Agents.FirstOrDefaultAsync(agents => agents.id == id);
            if (agent == null)
            {
                status = StatusCodes.Status404NotFound;
                return StatusCode(status, HttpUtils.Response(status, "agent not found"));
            }
            status = StatusCodes.Status200OK;
            return StatusCode(status, HttpUtils.Response(status, new { agent = agent }));
        }

        [HttpPut("{id}/pin")]
        public async Task<IActionResult> AgentPin(int id ,Location location)
        {
            int status;
            Agent agent = await this._context.Agents.FirstOrDefaultAsync(agents => agents.id == id);
            if (agent == null)
            {
                status = StatusCodes.Status404NotFound;
                return StatusCode(status, HttpUtils.Response(status, "agent not found"));
            }
            agent.location = location;
            status = StatusCodes.Status200OK;
            return StatusCode(status, HttpUtils.Response(status, new { agent = agent }));
        }



    }
}
