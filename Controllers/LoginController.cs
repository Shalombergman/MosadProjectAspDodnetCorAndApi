using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MosadApiServer.Models;

namespace MosadApiServer.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        [HttpPost]
        [Produces("Application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public IActionResult gdhfs([FromBody]LoginModel loginModel)
        {
            return StatusCode(

                StatusCodes.Status201Created,
                new { token = "gdsfg" }
                );
        }
    }
}
