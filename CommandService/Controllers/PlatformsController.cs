using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace PlatformsService.Controllers
{
    [Route("api/command/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase 
    {
        [HttpPost]
        public ActionResult TestInboundPlatformSync()
        {
            Console.WriteLine("-->POST request from Command is OK");
            
            return Ok();
        }
    }
}