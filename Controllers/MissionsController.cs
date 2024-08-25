using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MosadApiServer.Data;
using MosadApiServer.Enums;
using MosadApiServer.Models;
using MosadApiServer.Servises;
using MosadApiServer.Utils;

namespace MosadApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MissionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AgentsController> _logger;
        private readonly Coordinates _coordinates;
        private readonly Agent _agent;
        private readonly Target _target;

        public MissionsController(ILogger<AgentsController> logger, ApplicationDbContext context, Coordinates coordinates, Agent agent, Target target)
        {
            this._context = context;
            this._logger = logger;
            this._coordinates = coordinates;
            this._agent = agent;
            this._target = target;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllMission()
        {
            int status = StatusCodes.Status200OK;
            var agents = await this._context.Missions.Include(a => a.name).ToListAsync();
            return Ok(agents);
        }

        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Update(id)
        {

            int status;
            _agent = ServiceMission.OfferedMission();
            if (agent == null)
            {
                status = StatusCodes.Status404NotFound;
                return StatusCode(status, HttpUtils.Response(status, "agent not found"));
            }
        }



    }
}
