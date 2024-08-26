using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MosadApiServer.Data;
using MosadApiServer.Enums;
using MosadApiServer.Interfaces;
using MosadApiServer.Models;
using MosadApiServer.Servises;
using MosadApiServer.Utils;


namespace MosadApiServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TargetsController : Controller
{
    // GET

    private readonly ApplicationDbContext _context; 
    private readonly IServiceMoving _serviceMoving;
    private readonly ServiceMission _serviceMission;
    //public static Matrix matrix = new Matrix();
    public TargetsController(ApplicationDbContext context,  IServiceMoving serviceMoving, ServiceMission serviceMission)
    {
        int[,] matrix = new int[1000, 1000];

        this._context = context;
       
       
        this._serviceMoving = serviceMoving;
        this._serviceMission = serviceMission;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTargets()
    {
        int status = StatusCodes.Status200OK;
        var targets = await this._context.Targets.Include(t => t.location).ToListAsync();
        return Ok(targets);
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
            new { success = true, target = target.id }
        );
    }

    [HttpPut("{id}/pin")]
    public async Task<IActionResult> AgentPin(int id, Coordinates location)
    {
        int status;
        Target target = await this._context.Targets.FirstOrDefaultAsync(targets => targets.id == id);
        if (target == null)
        {
            status = StatusCodes.Status404NotFound;
            return StatusCode(status, HttpUtils.Response(status, "target not found"));
        }
        target.location = await this._serviceMoving.CreatPinlocation(location.x, location.y);
        status = StatusCodes.Status200OK;
        await this._context.SaveChangesAsync();
        await this._serviceMission.OfferedMission();
        return StatusCode(status, HttpUtils.Response(status, new { target = target }));
    }


    [HttpPut("{id}/move")]
    public async Task<IActionResult> AgentMove(int id, Direction direction)
    {
        Direction direction1 = direction;
        int status;
        Target target = await this._context.Targets.Include(t => t.location).FirstOrDefaultAsync(targets => targets.id == id);
        if (target == null)
        {
            status = StatusCodes.Status404NotFound;
            return StatusCode(status, HttpUtils.Response(status, "target not found"));
        }
        var coordinates = target.location;
        if (coordinates == null)
        {
            return BadRequest("target location is not set.");
        }
        target.location = await this._serviceMoving.Move(direction1,coordinates, 1, 1);
        status = StatusCodes.Status200OK;
        await this._context.SaveChangesAsync();
        await this._serviceMission.OfferedMission();
        return StatusCode(status, HttpUtils.Response(status, new { agent = target.location }));
    }
    
}