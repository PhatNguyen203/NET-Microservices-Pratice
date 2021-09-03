using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers
{
    [Route("api/command/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase 
    {
        [HttpPost]
        public ActionResult Post()
        {
            Console.WriteLine("-->POST request from Command is OK");
            
            return Ok();
        }
    }
}