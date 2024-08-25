using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MosadApiServer.Data;
using MosadApiServer.Enums;
using MosadApiServer.Models;
using MosadApiServer.Servises;
using MosadApiServer.Utils;


namespace MosadApiServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TaegetsController : Controller
{
    // GET

    private readonly ApplicationDbContext _context;
    private readonly ILogger<AgentsController> _logger;
    private readonly Coordinates _coordinates;
    //public static Matrix matrix = new Matrix();
    public TaegetsController(ILogger<AgentsController> logger, ApplicationDbContext context, Coordinates coordinates)
    {
        int[,] matrix = new int[1000, 1000];

        this._context = context;
        this._logger = logger;
        this._coordinates = coordinates;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTargets()
    {
        int status = StatusCodes.Status200OK;
        var agents = await this._context.Agents.Include(a => a.location).ToListAsync();
        return Ok(agents);
    }
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateAttack(Target target)
    {
        this._context.Targets.Add(target);
        await this._context.SaveChangesAsync();
        return StatusCode(
            StatusCodes.Status201Created,
            new { success = true, target = target }
        );
    }
    [HttpPut("{id}/move")]
    public async Task<IActionResult> AgentMove(int id, Direction direction)
    {
        Direction direction1 = direction;
        int status;
        Target target = await this._context.Targets.FirstOrDefaultAsync(targets => targets.id == id);
        if (target == null)
        {
            status = StatusCodes.Status404NotFound;
            return StatusCode(status, HttpUtils.Response(status, "agent not found"));
        }
        target.location = await ServiceMoving.Move(direction1, target.location, 1, 1);
        status = StatusCodes.Status200OK;
        return StatusCode(status, HttpUtils.Response(status, new { agent = target }));
    }
    [HttpPut("{id}/pin")]
    public async Task<IActionResult> AgentPin(int id, Coordinates location)
    {
        int status;
        Target target = await this._context.Targets.FirstOrDefaultAsync(targets => targets.id == id);
        if (target == null)
        {
            status = StatusCodes.Status404NotFound;
            return StatusCode(status, HttpUtils.Response(status, "agent not found"));
        }
        target.location = await ServiceMoving.CreatPinlocation(location.x, location.y);
        status = StatusCodes.Status200OK;
        return StatusCode(status, HttpUtils.Response(status, new { target = target }));
    }
}