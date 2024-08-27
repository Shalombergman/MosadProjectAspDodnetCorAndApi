using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using MosadApiServer.Dto;
using MosadApiServer.Servises;
using System.Collections.Immutable;

[Route("api/[controller]")]
[ApiController]
public class LoginController(JwtService jwtService) : ControllerBase
{


    private static readonly ImmutableList<string> allowedNames =
    [
        "SimulationServer", "MVCServer"
    ];


    
    [HttpPost]
    public ActionResult<string> Login([FromBody] LoginDto loginDto) =>
        allowedNames.Contains(loginDto.Name)
            ? Ok(jwtService.CreateToken(loginDto.Name))
            : BadRequest();
    

    [Authorize]
    [HttpPost("protected")]
    public ActionResult<string> Protected()
    {
        return Ok("Yay!");
    }
    
}
    

