using AutoMapper;
using CommandService.Data;
using CommandService.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace CommandService.Controllers
{
    [ApiController]
    [Route("api/commands/[controller]")]
    public class PlatformsController : ControllerBase
    {
        private readonly ICommandServiceRepo repo;
        private readonly IMapper mapper;

        public PlatformsController(ICommandServiceRepo repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }

        [HttpGet]
        public ActionResult<PlatformReadDto> GetAllPlatforms()
        {
            var platforms = repo.GetAllPlatforms();
            var platformsDto = mapper.Map<IEnumerable<PlatformReadDto>>(platforms);
            return Ok(platformsDto);
        }

        [HttpPost]
        public ActionResult TestInboundConnection()
        {
            Console.WriteLine("--> Inbound POST # Command Service");

            return Ok("Inbound test of from Platforms Controler");
        }
    }
}
