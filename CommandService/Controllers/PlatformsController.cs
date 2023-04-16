using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers
{
    [ApiController]
    [Route("api/commands/[controller]")]
    public class PlatformsController : ControllerBase
    {
        public PlatformsController()
        {

        }
        [HttpGet]
        public IActionResult GetAll()
        {

            return Ok();
        }
    }
}
